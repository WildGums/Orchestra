namespace Orchestra.Windows
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Management;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;

    public class DpiScale
    {
        public double X { get; set; }
        public double Y { get; set; }

        public override string ToString()
        {
            return $"X:{X} Y:{Y}";
        }
    }

    public class MonitorInfo
    {
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

        public int GetDpiAwareResolution()
        {
            throw new NotImplementedException();
        }

        public int GetDpiAwareWorkingArea()
        {
            throw new NotImplementedException();
        }

        public static MonitorInfo[] GetAllMonitors(bool throwErrorsForWrongAppManifest = true)
        {
            if (throwErrorsForWrongAppManifest)
            {
                // Step 1: check DPI awareness, must be PerMonitor
                NativeMethods.GetProcessDpiAwareness(Process.GetCurrentProcess().Handle, out DpiAwareness awareness);

                if (awareness != DpiAwareness.ProcessPerMonitor)
                {
                    throw new NotSupportedException("Application manifest is incorrect to retrieve reliable monitor info, see https://github.com/wildgums/orchestra/364 for more info");
                }

                // Step 2: check whether app is dpi-aware (should be false)

                // Or shall we just override?

                //var result = SetProcessDpiAwareness(PROCESS_DPI_AWARENESS.ProcessPerMonitor);
                //GetProcessDpiAwareness(Process.GetCurrentProcess().Handle, out awareness);
            }

            List<MonitorInfo> col = new List<MonitorInfo>();

            var adapterIndex = -1;

            List<DisplayDevice> videoAdapter = new List<DisplayDevice>();

            // Step 3: Go through Video Adapters Outputs
            while (true)
            {
                adapterIndex++;
                var adapter = DisplayDevice.Initialize();

                if (!NativeMethods.EnumDisplayDevices(null, (uint)adapterIndex, ref adapter, 0))
                {
                    break;
                }

                videoAdapter.Add(adapter);
            }

            Dictionary<DisplayDevice, string> displayDevicesToAdapter = new Dictionary<DisplayDevice, string>();

            // Step 4: Step into each device attached to every output and find out monitor devices
            foreach (var adapter in videoAdapter)
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

                    displayDevicesToAdapter.Add(display, adapter.DeviceName);
                }
            }

            List<NativeMonitorInfoEx> nativeMonitorInfos = new List<NativeMonitorInfoEx>();

            // Step 5: Enumerate available monitors
            NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData)
            {
                var mi = new NativeMonitorInfoEx();
                var success = NativeMethods.GetMonitorInfo(hMonitor, mi);

                if (success)
                {
                    nativeMonitorInfos.Add(mi);
                }

                return true;
            }, IntPtr.Zero);

            // Step 6: Compare native infos from GetMonitorInfo with display devices
            foreach (var key in displayDevicesToAdapter.Keys)
            {
                var displayDevice = key;
                var adapterDeviceName = displayDevicesToAdapter[displayDevice];

                var nativeInfo = nativeMonitorInfos.FirstOrDefault(x => string.Equals(x.GetDeviceName(), adapterDeviceName));

                var di = new MonitorInfo
                {
                    DeviceName = nativeInfo.GetDeviceName(),
                    ScreenWidth = nativeInfo.Monitor.GetWidth().ToString(),
                    ScreenHeight = nativeInfo.Monitor.GetHeight().ToString(),
                    MonitorArea = nativeInfo.Monitor.ToInt32Rect(),
                    WorkingArea = nativeInfo.Work.ToInt32Rect(),
                    Availability = nativeInfo.Flags.ToString(),
                    IsPrimary = nativeInfo.Flags == 1,
                    FriendlyName = displayDevice.DeviceString,
                    DeviceNameFull = displayDevice.DeviceName,
                    AdapterDeviceName = adapterDeviceName
                };

                col.Add(di);
            }

            return col.ToArray();
        }

        public static MonitorInfo GetPrimaryMonitor()
        {
            return GetAllMonitors().FirstOrDefault(x => x.IsPrimary);
        }

        public static MonitorInfo GetMonitorFromWindow(Window window)
        {
            // Get screen from window
            var windowInteropHelper = new WindowInteropHelper(window);
            return GetMonitorFromWindowHandle(windowInteropHelper.Handle);
        }

        public static MonitorInfo GetMonitorFromWindowHandle(IntPtr handle)
        {
            // Get screen from window handle
            var monitorHandle = NativeMethods.MonitorFromWindow(handle, 0);

            var nativeInfo = new NativeMonitorInfoEx();
           
            var success = NativeMethods.GetMonitorInfo(monitorHandle, nativeInfo);

            if(!success)
            {
                return null;
            }

            // note: can this cause issues when monitor mirror, probably need to check DisplayDeviceStateFlag?
            var outputDevice = GetOutputDevicesForDevice(nativeInfo.GetDeviceName()).FirstOrDefault();

            var di = new MonitorInfo
            {
                DeviceName = nativeInfo.GetDeviceName(),
                ScreenWidth = nativeInfo.Monitor.GetWidth().ToString(),
                ScreenHeight = nativeInfo.Monitor.GetHeight().ToString(),
                MonitorArea = nativeInfo.Monitor.ToInt32Rect(),
                WorkingArea = nativeInfo.Work.ToInt32Rect(),
                Availability = nativeInfo.Flags.ToString(),
                IsPrimary = nativeInfo.Flags == 1,
                FriendlyName = outputDevice.DeviceString,
                DeviceNameFull = outputDevice.DeviceName,
                AdapterDeviceName = nativeInfo.GetDeviceName()
            };

            return di;
        }

        private static DisplayDevice[] GetOutputDevicesForDevice(string adapterDeviceName)
        {
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

        internal static class NativeMethods
        {
            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            public static extern bool GetMonitorInfo(IntPtr hmonitor, [In, Out] NativeMonitorInfoEx info);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            public static extern bool EnumDisplayDevices(string deviceName, uint deviceNumber, ref DisplayDevice displayDevice, uint flags);

            [DllImport("user32.dll")]
            public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

            public delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

            [DllImport("user32.dll")]
            public static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

            [DllImport("shcore.dll")]
            private static extern IntPtr GetDpiForMonitor([In] IntPtr hmonitor, [In] DpiType dpiType, [Out] out uint dpiX, [Out] out uint dpiY);

            [DllImport("shcore.dll", SetLastError = true)]
            public static extern bool SetProcessDpiAwareness(DpiAwareness awareness);

            [DllImport("shcore.dll", SetLastError = true)]
            public static extern void GetProcessDpiAwareness(IntPtr hprocess, out DpiAwareness awareness);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        internal class NativeMonitorInfoEx
        {
            public int Size = Marshal.SizeOf(typeof(NativeMonitorInfoEx));
            public NativeRectangle Monitor = new NativeRectangle();
            public NativeRectangle Work = new NativeRectangle();
            public int Flags = 0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] Device = new char[32];

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

        public enum DpiType
        {
            Effective = 0,
            Angular = 1,
            Raw = 2,
        }
    }
}
