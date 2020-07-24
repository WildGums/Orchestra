namespace Orchestra.Windows
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;
    using Catel;
    using Catel.Logging;

    public class DpiScale
    {
        private const double BasicAbsoluteDpi = 96;

        public double X { get; set; }
        public double Y { get; set; }

        public void SetScaleFromAbsolute(uint absoluteDpiX, uint absoluteDpiY)
        {
            X = absoluteDpiX / BasicAbsoluteDpi;
            Y = absoluteDpiY / BasicAbsoluteDpi;
        }

        public override string ToString()
        {
            return $"X:{X} Y:{Y}";
        }
    }

    public class MonitorInfo
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public string Id { get; set; }

        public bool IsPrimary { get; set; }

        public string Availability { get; set; }

        public string FriendlyName { get; set; }

        public string DeviceName { get; set; }

        public string DeviceNameFull { get; set; }

        public string AdapterDeviceName { get; set; }

        public string ScreenHeight { get; set; }

        public string ScreenWidth { get; set; }

        public Int32Rect MonitorArea { get; set; }

        public Int32Rect WorkingArea { get; set; }

        public DpiScale DpiScale { get; set; }

        public Rect GetDpiAwareResolution()
        {
            return new Rect(MonitorArea.X, MonitorArea.Y, MonitorArea.Width * DpiScale.X, MonitorArea.Height * DpiScale.Y);
        }

        public Rect GetDpiAwareWorkingArea()
        {
            return new Rect(WorkingArea.X * DpiScale.X, WorkingArea.Y * DpiScale.Y, (int)(WorkingArea.Width * DpiScale.X), (int)(WorkingArea.Height * DpiScale.Y));
        }

        public static MonitorInfo[] GetAllMonitors(bool throwErrorsForWrongAppManifest = true)
        {
            if (throwErrorsForWrongAppManifest)
            {
                // Step 1: check DPI awareness, must be PerMonitor
                NativeMethods.GetProcessDpiAwareness(Process.GetCurrentProcess().Handle, out var awareness);

                if (awareness != DpiAwareness.ProcessPerMonitor)
                {
                    throw Log.ErrorAndCreateException<NotSupportedException>("Application manifest is incorrect to retrieve reliable monitor info, see https://github.com/wildgums/orchestra/364 for more info");
                }

                // Step 2: check whether app is dpi-aware (should be false)

                // Or shall we just override?

                //var result = SetProcessDpiAwareness(PROCESS_DPI_AWARENESS.ProcessPerMonitor);
                //GetProcessDpiAwareness(Process.GetCurrentProcess().Handle, out awareness);
            }

            var monitorInfos = new List<MonitorInfo>();
            var videoAdapters = new List<DisplayDevice>();

            var adapterIndex = -1;

            // Step 3: Go through Video Adapters Outputs
            while (true)
            {
                adapterIndex++;
                var adapter = DisplayDevice.Initialize();

                if (!NativeMethods.EnumDisplayDevices(null, (uint)adapterIndex, ref adapter, 0))
                {
                    break;
                }

                videoAdapters.Add(adapter);
            }

            var displayDevicesToAdapter = new Dictionary<DisplayDevice, string>();

            // Step 4: Step into each device attached to every output and find out monitor devices
            foreach (var adapter in videoAdapters)
            {
                var displayIndex = -1;

                while (true)
                {
                    displayIndex++;

                    var display = DisplayDevice.Initialize();

                    if (!NativeMethods.EnumDisplayDevices(adapter.DeviceName, (uint)displayIndex, ref display, 1))
                    {
                        break;
                    }

                    displayDevicesToAdapter[display] = adapter.DeviceName;
                }
            }

            var nativeMonitorInfos = new Dictionary<NativeMonitorInfoEx, IntPtr>();

            var displayConfigs = GetDisplayConfigs();

            // Step 5: Enumerate available monitors
            NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData)
            {
                var monitorInfo = NativeMonitorInfoEx.Initialize();
                var success = NativeMethods.GetMonitorInfo(hMonitor, ref monitorInfo);

                if (success)
                {
                    nativeMonitorInfos.Add(monitorInfo, hMonitor); // add handle, we can use it later to get dpi
                }

                return true;
            }, IntPtr.Zero);

            // Step 6: Compare native infos from GetMonitorInfo with display devices
            foreach (var keyValuePair in displayDevicesToAdapter)
            {
                var displayDevice = keyValuePair.Key;
                var adapterDeviceName = keyValuePair.Value;

                var nativeMonitorInfoAndHandle = nativeMonitorInfos.FirstOrDefault(x => string.Equals(x.Key.GetDeviceName(), adapterDeviceName));
                var nativeMonitorInfo = nativeMonitorInfoAndHandle.Key;
                var monitorHandle = nativeMonitorInfoAndHandle.Value;

                // Get dpi
                var dpiScale = new DpiScale();

                NativeMethods.GetDpiForMonitor(monitorHandle, DpiType.Effective, out var dpiScaleX, out var dpiScaleY);

                if (dpiScaleX > 0 && dpiScaleY > 0)
                {
                    dpiScale.SetScaleFromAbsolute(dpiScaleX, dpiScaleY);
                }

                var di = new MonitorInfo
                {
                    DeviceName = nativeMonitorInfo.GetDeviceName(),
                    ScreenWidth = nativeMonitorInfo.Monitor.GetWidth().ToString(),
                    ScreenHeight = nativeMonitorInfo.Monitor.GetHeight().ToString(),
                    MonitorArea = nativeMonitorInfo.Monitor.ToInt32Rect(),
                    WorkingArea = nativeMonitorInfo.Work.ToInt32Rect(),
                    Availability = nativeMonitorInfo.Flags.ToString(),
                    IsPrimary = nativeMonitorInfo.Flags == 1,
                    FriendlyName = displayConfigs.FirstOrDefault(c => string.Equals(c.MonitorDevicePath, displayDevice.DeviceId)).MonitorFriendDeviceName,
                    DeviceNameFull = displayDevice.DeviceName,
                    AdapterDeviceName = adapterDeviceName,
                    DpiScale = dpiScale
                };

                monitorInfos.Add(di);
            }

            return monitorInfos.ToArray();
        }

        public static MonitorInfo GetPrimaryMonitor()
        {
            return GetAllMonitors().FirstOrDefault(x => x.IsPrimary);
        }

        public static MonitorInfo GetMonitorFromWindow(Window window)
        {
            Argument.IsNotNull(() => window);

            var windowInteropHelper = new WindowInteropHelper(window);
            return GetMonitorFromWindowHandle(windowInteropHelper.Handle);
        }

        public static MonitorInfo GetMonitorFromWindowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw Log.ErrorAndCreateException((string errorMessage) => new ArgumentException(errorMessage, nameof(handle)), "Pointer has been initialized to zero");
            }
            // Get screen from window handle
            var monitorHandle = NativeMethods.MonitorFromWindow(handle, 0);

            var nativeInfo = NativeMonitorInfoEx.Initialize();

            var success = NativeMethods.GetMonitorInfo(monitorHandle, ref nativeInfo);

            if (!success)
            {
                return null;
            }

            // note: can this cause issues when monitor mirror, probably need to check DisplayDeviceStateFlag?
            var outputDevice = GetOutputDevicesForDevice(nativeInfo.GetDeviceName())?.FirstOrDefault();

            if (outputDevice is null || outputDevice.Value.Size == 0)
            {
                return null;
            }

            var displayConfigs = GetDisplayConfigs();

            // Get dpi
            var dpiScale = new DpiScale();

            NativeMethods.GetDpiForMonitor(monitorHandle, DpiType.Effective, out var dpiScaleX, out var dpiScaleY);

            if (dpiScaleX > 0 && dpiScaleY > 0)
            {
                dpiScale.SetScaleFromAbsolute(dpiScaleX, dpiScaleY);
            }

            var di = new MonitorInfo
            {
                DeviceName = nativeInfo.GetDeviceName(),
                ScreenWidth = nativeInfo.Monitor.GetWidth().ToString(),
                ScreenHeight = nativeInfo.Monitor.GetHeight().ToString(),
                MonitorArea = nativeInfo.Monitor.ToInt32Rect(),
                WorkingArea = nativeInfo.Work.ToInt32Rect(),
                Availability = nativeInfo.Flags.ToString(),
                IsPrimary = nativeInfo.Flags == 1,
                FriendlyName = displayConfigs.FirstOrDefault(c => string.Equals(c.MonitorDevicePath, outputDevice.Value.DeviceId)).MonitorFriendDeviceName,
                DeviceNameFull = outputDevice.Value.DeviceName,
                AdapterDeviceName = nativeInfo.GetDeviceName(),
                DpiScale = dpiScale
            };

            return di;
        }

        private static DisplayDevice[] GetOutputDevicesForDevice(string adapterDeviceName)
        {
            Argument.IsNotNullOrEmpty(() => adapterDeviceName);

            var displayIndex = -1;
            var outputDevices = new List<DisplayDevice>();

            while (true)
            {
                displayIndex++;

                var display = DisplayDevice.Initialize();

                if (!NativeMethods.EnumDisplayDevices(adapterDeviceName, (uint)displayIndex, ref display, 1))
                {
                    break;
                }

                outputDevices.Add(display);
            }

            return outputDevices.ToArray();
        }

        private static DisplayConfigTargetDeviceName[] GetDisplayConfigs()
        {
            var error = NativeMethods.GetDisplayConfigBufferSizes(QueryDeviceConfigFlags.QdcOnlyActivePaths, out var pathInfoElementsCount, out var modeInfoElementsCount);

            if (error != 0)
            {
                throw Log.ErrorAndCreateException<Win32Exception>($"Function {nameof(NativeMethods.GetDisplayConfigBufferSizes)} returns error code '{error}'");
            }

            var displayConfigs = new List<DisplayConfigTargetDeviceName>();

            var displayConfigPathInfos = new DisplayConfigPathInfo[pathInfoElementsCount];
            var displayConfigModeInfos = new DisplayConfigModeInfo[modeInfoElementsCount];

            error = NativeMethods.QueryDisplayConfig(QueryDeviceConfigFlags.QdcOnlyActivePaths, ref pathInfoElementsCount, displayConfigPathInfos, ref modeInfoElementsCount,
                displayConfigModeInfos, IntPtr.Zero);

            if (error != 0)
            {
                throw Log.ErrorAndCreateException<Win32Exception>($"Function {nameof(NativeMethods.QueryDisplayConfig)} returns error code '{error}'");
            }

            foreach (var pathInfo in displayConfigPathInfos)
            {
                var adapterName = new DisplayConfigAdapterName(pathInfo.TargetInfo.AdapterId, DisplayConfig.DeviceInfoType.DisplayConfigDeviceInfoGetAdapterName);

                var targetDeviceName = new DisplayConfigTargetDeviceName()
                {
                    Header = DisplayConfigDeviceInfoHeader.Initialize(pathInfo.TargetInfo.AdapterId,
                    pathInfo.TargetInfo.Id,
                    DisplayConfig.DeviceInfoType.DisplayConfigDeviceInfoGetTargetName,
                    (uint)Marshal.SizeOf<DisplayConfigTargetDeviceName>())
                };

                error = (int)NativeMethods.DisplayConfigGetDeviceInfo(ref targetDeviceName);

                try
                {
                    if (error != 0)
                    {
                        throw Log.ErrorAndCreateException<Win32Exception>($"Function {nameof(NativeMethods.DisplayConfigGetDeviceInfo)} returns error code '{error}'");
                    }

                    displayConfigs.Add(targetDeviceName);
                }
                catch (Win32Exception ex)
                {
                    Log.Error(ex);
                }
            }

            return displayConfigs.ToArray();
        }

        internal static class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern int GetDisplayConfigBufferSizes(QueryDeviceConfigFlags flags, out uint numPathArrayElements, out uint numModeInfoArrayElements);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            public static extern bool GetMonitorInfo(IntPtr hMonitor, ref NativeMonitorInfoEx info);

            [DllImport("user32")]
            public static extern Win32Status DisplayConfigGetDeviceInfo(ref DisplayConfigAdapterName deviceName);

            [DllImport("user32")]
            public static extern Win32Status DisplayConfigGetDeviceInfo(ref DisplayConfigTargetDeviceName deviceName);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            public static extern bool EnumDisplayDevices(string deviceName, uint deviceNumber, ref DisplayDevice displayDevice, uint flags);

            [DllImport("user32.dll")]
            public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

            public delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

            [DllImport("user32.dll")]
            public static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

            [DllImport("user32.dll")]
            public static extern int QueryDisplayConfig([In] QueryDeviceConfigFlags flags, [In, Out] ref uint numPathArrayElements, [Out] DisplayConfigPathInfo[] pathInfoArray,
            [In, Out] ref uint numModeInfoArrayElements, [Out] DisplayConfigModeInfo[] modeInfoArray, IntPtr currentTopologyId);

            [DllImport("shcore.dll")]
            public static extern bool GetDpiForMonitor([In] IntPtr hMonitor, [In] DpiType dpiType, [Out] out uint dpiX, [Out] out uint dpiY);

            [DllImport("shcore.dll", SetLastError = true)]
            public static extern void GetProcessDpiAwareness(IntPtr hProcess, out DpiAwareness awareness);

            [DllImport("shcore.dll", SetLastError = true)]
            public static extern bool SetProcessDpiAwareness(DpiAwareness awareness);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        internal struct NativeMonitorInfoEx
        {
            public uint Size;
            public NativeRectangle Monitor;
            public NativeRectangle Work;
            public int Flags;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] Device;

            public static NativeMonitorInfoEx Initialize()
            {
                return new NativeMonitorInfoEx()
                {
                    Size = (uint)Marshal.SizeOf(typeof(NativeMonitorInfoEx)),
                    Monitor = new NativeRectangle(),
                    Work = new NativeRectangle(),
                    Flags = 0,
                    Device = new char[32]
                };
            }

            public string GetDeviceName()
            {
                return new string(Device.Where(c => c != '\0').ToArray());
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct DisplayDevice
        {
            [MarshalAs(UnmanagedType.U4)]
            internal uint Size;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public readonly string DeviceName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public readonly string DeviceString;

            [MarshalAs(UnmanagedType.U4)]
            public readonly DisplayDeviceStateFlags StateFlags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public readonly string DeviceId;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public readonly string DeviceKey;

            public static DisplayDevice Initialize()
            {
                return new DisplayDevice
                {
                    Size = (uint)Marshal.SizeOf(typeof(DisplayDevice))
                };
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        internal struct NativeRectangle
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public NativeRectangle(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public int GetWidth()
            {
                return Right - Left;
            }

            public int GetHeight()
            {
                return Bottom - Top;
            }

            public Int32Rect ToInt32Rect()
            {
                return new Int32Rect
                {
                    X = Left,
                    Y = Top,
                    Width = GetWidth(),
                    Height = GetHeight()
                };
            }

            public override string ToString()
            {
                return $"Left:{Left} Top:{Top} Right:{Right} Bottom:{Bottom}";
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct DisplayConfigModeInfo
        {
            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(0)]
            public readonly DisplayConfig.ModeInfoType InfoType;

            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(4)]
            public readonly uint Id;

            [MarshalAs(UnmanagedType.Struct)]
            [FieldOffset(8)]
            public readonly LUID AdapterId;

            [MarshalAs(UnmanagedType.Struct)]
            [FieldOffset(16)]
            public readonly DisplayConfigModeInfoUnion TargetMode;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct DisplayConfigModeInfoUnion
        {
            [FieldOffset(0)]
            public DisplayConfigTargetMode TargetMode;
            [FieldOffset(0)]
            public DisplayConfigSourceMode SourceMode;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DisplayConfigSourceMode
        {
            public uint Width;
            public uint Height;
            public DisplayConfig.PixelFormat PixelFormat;
            public PointL Position;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DisplayConfigTargetMode
        {
            public DisplayConfigVideoSignalInfo TargetVideoSignalInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DisplayConfigVideoSignalInfo
        {
            public ulong PixelRate;
            public DisplayConfigRational HSyncFreq;
            public DisplayConfigRational VSyncFreq;
            public DisplayConfig2DRegion ActiveSize;
            public DisplayConfig2DRegion TotalSize;
            public uint VideoStandard;
            public DisplayConfig.ScanLineOrdering ScanLineOrdering;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DisplayConfigRational
        {
            public uint Numerator;
            public uint Denominator;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DisplayConfig2DRegion
        {
            public uint Cx;
            public uint Cy;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct DisplayConfigAdapterName
        {
            // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
            [MarshalAs(UnmanagedType.Struct)]
            private readonly DisplayConfigDeviceInfoHeader _header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public readonly string AdapterDevicePath;

            public DisplayConfigAdapterName(LUID adapter, DisplayConfig.DeviceInfoType deviceInfoType) : this()
            {
                _header = DisplayConfigDeviceInfoHeader.Initialize(adapter, deviceInfoType, (uint)Marshal.SizeOf<DisplayConfigAdapterName>());
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PointL
        {
            private readonly int _x;
            private readonly int _y;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DisplayConfigPathInfo
        {
            public DisplayConfigPathSourceInfo SourceInfo;
            public DisplayConfigPathTargetInfo TargetInfo;
            public uint Flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DisplayConfigPathSourceInfo
        {
            public LUID AdapterId;
            public uint Id;
            public uint ModeInfoIdx;
            public uint StatusFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DisplayConfigPathTargetInfo
        {
            public LUID AdapterId;
            public uint Id;
            public uint ModeInfoIdx;
            private readonly DisplayConfig.VideoOutputTechnology _outputTechnology;
            private readonly DisplayConfig.Rotation _rotation;
            private readonly DisplayConfig.Scaling _scaling;
            private readonly DisplayConfigRational _refreshRate;
            private readonly DisplayConfig.ScanLineOrdering _scanLineOrdering;
            public bool TargetAvailable;
            public uint StatusFlags;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct DisplayConfigTargetDeviceName
        {
            public DisplayConfigDeviceInfoHeader Header;
            public DisplayConfigTargetDeviceNameFlags Flags;
            public DisplayConfig.VideoOutputTechnology OutputTechnology;
            public ushort EditManufactureId;
            public ushort EditProductCodeId;
            public uint ConnectorInstance;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string MonitorFriendDeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string MonitorDevicePath;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DisplayConfigDeviceInfoHeader
        {
            public DisplayConfig.DeviceInfoType Type;
            public uint Size;
            public LUID AdapterId;
            public uint Id;

            public static DisplayConfigDeviceInfoHeader Initialize(LUID adapterId, DisplayConfig.DeviceInfoType requestType, uint size)
            {
                return new DisplayConfigDeviceInfoHeader()
                {
                    AdapterId = adapterId,
                    Type = requestType,
                    Size = size
                };
            }

            public static DisplayConfigDeviceInfoHeader Initialize(LUID adapterId, uint targetId, DisplayConfig.DeviceInfoType requestType, uint size)
            {
                return new DisplayConfigDeviceInfoHeader()
                {
                    AdapterId = adapterId,
                    Id = targetId,
                    Type = requestType,
                    Size = size
                };
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct LUID
        {
            public uint LowPart;
            public int HighPart;

            public override string ToString()
            {
                return string.Format("{0}{1}", LowPart, HighPart);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DisplayConfigTargetDeviceNameFlags
        {
            public uint Value;
        }

        [Flags]
        internal enum DisplayDeviceStateFlags : uint
        {
            /// <summary>
            ///     The device is part of the desktop.
            /// </summary>
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,

            /// <summary>
            ///     The device is part of the desktop.
            /// </summary>
            PrimaryDevice = 0x4,

            /// <summary>
            ///     Represents a pseudo device used to mirror application drawing for remoting or other purposes.
            /// </summary>
            MirroringDriver = 0x8,

            /// <summary>
            ///     The device is VGA compatible.
            /// </summary>
            VGACompatible = 0x10,

            /// <summary>
            ///     The device is removable; it cannot be the primary display.
            /// </summary>
            Removable = 0x20,

            /// <summary>
            ///     The device has more display modes than its output devices support.
            /// </summary>
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        [Flags]
        public enum DpiAwareness
        {
            /// <summary>
            ///     DPI unaware. This process does not scale for DPI changes and is always assumed to have a scale factor of 100% (96 DPI). 
            ///     It will be automatically scaled by the system on any other DPI setting.
            /// </summary>
            Unaware = 0x0,

            /// <summary>
            ///     System DPI aware. This process does not scale for DPI changes. 
            ///     It will query for the DPI once and use that value for the lifetime of the process. If the DPI changes, the process will not adjust to the new DPI value. 
            ///     It will be automatically scaled up or down by the system when the DPI changes from the system value.
            /// </summary>
            System = 0x1,

            /// <summary>
            ///     Per monitor DPI aware. This process checks for the DPI when it is created and adjusts the scale factor whenever the DPI changes. 
            ///     These processes are not automatically scaled by the system.
            /// </summary>
            ProcessPerMonitor = 0x2
        }

        [Flags]
        public enum DpiAwarenessContext
        {
            /// <summary>
            ///     DPI unaware. This window does not scale for DPI changes and is always assumed to have a scale factor of 100% (96 DPI). 
            ///     It will be automatically scaled by the system on any other DPI setting.
            /// </summary>
            Unaware = 0,

            /// <summary>
            ///     System DPI aware. This window does not scale for DPI changes. 
            ///     It will query for the DPI once and use that value for the lifetime of the process. If the DPI changes, the process will not adjust to the new DPI value. 
            ///     It will be automatically scaled up or down by the system when the DPI changes from the system value.
            /// </summary>
            System = 1,

            /// <summary>
            ///     Per monitor DPI aware. This window checks for the DPI when it is created and adjusts the scale factor whenever the DPI changes. 
            ///     These processes are not automatically scaled by the system.
            /// </summary>
            ProcessPerMonitor = 2,

            /// <summary>
            ///     Also known as Per Monitor v2. An advancement over the original per-monitor DPI awareness mode, 
            ///     which enables applications to access new DPI-related scaling behaviors on a per top-level window basis.
            /// </summary>
            ProcessPerMonitorV2 = 3,

            /// <summary>
            ///     DPI unaware with improved quality of GDI-based content. 
            ///     This mode behaves similarly to DPI_AWARENESS_CONTEXT_UNAWARE, but also enables the system to automatically improve the 
            ///     rendering quality of text and other GDI-based primitives when the window is displayed on a high-DPI monitor.
            /// </summary>
            UnawareGdiScaled = 4
        }

        [Flags]
        internal enum QueryDeviceConfigFlags : uint
        {
            QdcAllPaths = 0x00000001,
            QdcOnlyActivePaths = 0x00000002,
            QdcDatabaseCurrent = 0x00000004
        }

        internal enum Win32Status
        {
            Success = 0x0,
            ErrorInsufficientBuffer = 0x7A
        }

        public enum DpiType
        {
            Effective = 0,
            Angular = 1,
            Raw = 2,
        }
    }
}
