﻿namespace Orchestra.Windows
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
    using Orchestra.Win32;

    public partial class MonitorInfo : IMonitorInfo
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public MonitorInfo()
        {
            Id = string.Empty;
            Availability = string.Empty;
            FriendlyName = string.Empty;
            DeviceName = string.Empty;
            DeviceNameFull = string.Empty;
            AdapterDeviceName = string.Empty;
            ScreenHeight = string.Empty;
            ScreenWidth = string.Empty;
            ManufactureCode = string.Empty;
            DpiScale = new DpiScale();
        }

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

        public string ManufactureCode { get; set; }

        public ushort ProductCodeId { get; set; }

        public Rect GetDpiAwareResolution()
        {
            return new Rect(MonitorArea.X, MonitorArea.Y, MonitorArea.Width * DpiScale.X, MonitorArea.Height * DpiScale.Y);
        }

        public Rect GetDpiAwareWorkingArea()
        {
            return new Rect(WorkingArea.X * DpiScale.X, WorkingArea.Y * DpiScale.Y, (int)(WorkingArea.Width * DpiScale.X), (int)(WorkingArea.Height * DpiScale.Y));
        }

        public override string ToString()
        {
            return $"{Id} {FriendlyName} {DeviceName}";
        }

        public static IMonitorInfo[] GetAllMonitors(bool throwErrorsForWrongAppManifest = true)
        {
            if (throwErrorsForWrongAppManifest)
            {
                // Step 1: check DPI awareness, must be PerMonitor
                Shcore.GetProcessDpiAwareness(Process.GetCurrentProcess().Handle, out var awareness);

                if (awareness != DpiAwareness.ProcessPerMonitor)
                {
                    throw Log.ErrorAndCreateException<NotSupportedException>("Application manifest is incorrect to retrieve reliable monitor info, see https://github.com/wildgums/orchestra/364 for more info");
                }

                // Step 2: check whether app is dpi-aware (should be false)

                // Or shall we just override?

                //var result = SetProcessDpiAwareness(PROCESS_DPI_AWARENESS.ProcessPerMonitor);
                //GetProcessDpiAwareness(Process.GetCurrentProcess().Handle, out awareness);
            }

            var monitorInfos = new List<IMonitorInfo>();
            var videoAdapters = new List<DisplayDevice>();

            var adapterIndex = -1;

            // Step 3: Go through Video Adapters Outputs
            while (true)
            {
                adapterIndex++;
                var adapter = DisplayDevice.Initialize();

                if (!User32.EnumDisplayDevices(null, (uint)adapterIndex, ref adapter, 0))
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

                    if (!User32.EnumDisplayDevices(adapter.DeviceName, (uint)displayIndex, ref display, 1))
                    {
                        break;
                    }

                    displayDevicesToAdapter[display] = adapter.DeviceName;
                }
            }

            var nativeMonitorInfos = new Dictionary<NativeMonitorInfoEx, IntPtr>();

            var displayConfigs = GetDisplayConfigs();

            // Step 5: Enumerate available monitors
            User32.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
            {
                var monitorInfo = NativeMonitorInfoEx.Initialize();
                var success = User32.GetMonitorInfo(hMonitor, ref monitorInfo);

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

                if (nativeMonitorInfo == default)
                {
                    // then this monitor is disabled or in some invalid state
                    continue;
                }

                var monitorHandle = nativeMonitorInfoAndHandle.Value;

                // Get dpi
                var dpiScale = new DpiScale();

                Shcore.GetDpiForMonitor(monitorHandle, DpiType.Effective, out var dpiScaleX, out var dpiScaleY);

                if (dpiScaleX > 0 && dpiScaleY > 0)
                {
                    dpiScale.SetScaleFromAbsolute(dpiScaleX, dpiScaleY);
                }

                var matchedDisplayConfig = displayConfigs.FirstOrDefault(c => string.Equals(c.MonitorDevicePath, displayDevice.DeviceId));

                var di = new MonitorInfo
                {
                    DeviceName = nativeMonitorInfo.GetDeviceName() ?? string.Empty,
                    ScreenWidth = nativeMonitorInfo.Monitor.GetWidth().ToString(),
                    ScreenHeight = nativeMonitorInfo.Monitor.GetHeight().ToString(),
                    MonitorArea = nativeMonitorInfo.Monitor.ToInt32Rect(),
                    WorkingArea = nativeMonitorInfo.Work.ToInt32Rect(),
                    Availability = nativeMonitorInfo.Flags.ToString(),
                    IsPrimary = nativeMonitorInfo.Flags == 1,
                    FriendlyName = string.IsNullOrEmpty(matchedDisplayConfig.MonitorFriendDeviceName) ? displayDevice.DeviceString : matchedDisplayConfig.MonitorFriendDeviceName,
                    DeviceNameFull = displayDevice.DeviceName,
                    AdapterDeviceName = adapterDeviceName,
                    DpiScale = dpiScale,
                    ProductCodeId = matchedDisplayConfig.EDIDProductCodeId,
                    ManufactureCode = ConvertEDIDManufactureIdToCode(matchedDisplayConfig.EDIDManufactureId)
                };

                monitorInfos.Add(di);
            }

            return monitorInfos.ToArray();
        }

        public static IMonitorInfo? GetPrimaryMonitor()
        {
            return GetAllMonitors().FirstOrDefault(x => x.IsPrimary);
        }

        public static IMonitorInfo? GetMonitorFromWindow(Window window)
        {
            ArgumentNullException.ThrowIfNull(window);

            var windowInteropHelper = new WindowInteropHelper(window);
            return GetMonitorFromWindowHandle(windowInteropHelper.Handle);
        }

        public static IMonitorInfo? GetMonitorFromWindowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw Log.ErrorAndCreateException((string errorMessage) => new ArgumentException(errorMessage, nameof(handle)), "Pointer has been initialized to zero");
            }

            // Get screen from window handle
            var monitorHandle = User32.MonitorFromWindow(handle, 0);

            var nativeInfo = NativeMonitorInfoEx.Initialize();

            var success = User32.GetMonitorInfo(monitorHandle, ref nativeInfo);

            if (!success)
            {
                return null;
            }

            // note: can this cause issues when monitor mirror, probably need to check DisplayDeviceStateFlag?
            var outputDevice = GetOutputDevicesForDevice(nativeInfo.GetDeviceName() ?? string.Empty)?.FirstOrDefault();

            if (outputDevice is null || outputDevice.Value.Size == 0)
            {
                return null;
            }

            var displayConfigs = GetDisplayConfigs();

            // Get dpi
            var dpiScale = new DpiScale();

            Shcore.GetDpiForMonitor(monitorHandle, DpiType.Effective, out var dpiScaleX, out var dpiScaleY);

            if (dpiScaleX > 0 && dpiScaleY > 0)
            {
                dpiScale.SetScaleFromAbsolute(dpiScaleX, dpiScaleY);
            }

            var matchedDisplayConfig = displayConfigs.FirstOrDefault(c => string.Equals(c.MonitorDevicePath, outputDevice.Value.DeviceId));

            var di = new MonitorInfo
            {
                DeviceName = nativeInfo.GetDeviceName() ?? string.Empty,
                ScreenWidth = nativeInfo.Monitor.GetWidth().ToString(),
                ScreenHeight = nativeInfo.Monitor.GetHeight().ToString(),
                MonitorArea = nativeInfo.Monitor.ToInt32Rect(),
                WorkingArea = nativeInfo.Work.ToInt32Rect(),
                Availability = nativeInfo.Flags.ToString(),
                IsPrimary = nativeInfo.Flags == 1,
                FriendlyName = string.IsNullOrEmpty(matchedDisplayConfig.MonitorFriendDeviceName) ? outputDevice.Value.DeviceString : matchedDisplayConfig.MonitorFriendDeviceName,
                DeviceNameFull = outputDevice.Value.DeviceName,
                AdapterDeviceName = nativeInfo.GetDeviceName() ?? string.Empty,
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

                if (!User32.EnumDisplayDevices(adapterDeviceName, (uint)displayIndex, ref display, 1))
                {
                    break;
                }

                outputDevices.Add(display);
            }

            return outputDevices.ToArray();
        }

        private static DisplayConfigTargetDeviceName[] GetDisplayConfigs()
        {
            var error = User32.GetDisplayConfigBufferSizes(QueryDeviceConfigFlags.QdcOnlyActivePaths, out var pathInfoElementsCount, out var modeInfoElementsCount);

            if (error != 0)
            {
                throw Log.ErrorAndCreateException<Win32Exception>($"Function {nameof(User32.GetDisplayConfigBufferSizes)} returns error code '{error}'");
            }

            var displayConfigs = new List<DisplayConfigTargetDeviceName>();

            var displayConfigPathInfos = new DisplayConfigPathInfo[pathInfoElementsCount];
            var displayConfigModeInfos = new DisplayConfigModeInfo[modeInfoElementsCount];

            error = User32.QueryDisplayConfig(QueryDeviceConfigFlags.QdcOnlyActivePaths, ref pathInfoElementsCount, displayConfigPathInfos, ref modeInfoElementsCount,
                displayConfigModeInfos, IntPtr.Zero);

            if (error != 0)
            {
                throw Log.ErrorAndCreateException<Win32Exception>($"Function {nameof(User32.QueryDisplayConfig)} returns error code '{error}'");
            }

            foreach (var pathInfo in displayConfigPathInfos)
            {
                var adapterName = new DisplayConfigAdapterName(pathInfo.TargetInfo.AdapterId, DeviceInfoType.DisplayConfigDeviceInfoGetAdapterName);

                var targetDeviceName = new DisplayConfigTargetDeviceName()
                {
                    Header = DisplayConfigDeviceInfoHeader.Initialize(pathInfo.TargetInfo.AdapterId,
                    pathInfo.TargetInfo.Id,
                    DeviceInfoType.DisplayConfigDeviceInfoGetTargetName,
                    (uint)Marshal.SizeOf<DisplayConfigTargetDeviceName>())
                };

                error = (int)User32.DisplayConfigGetDeviceInfo(ref targetDeviceName);

                try
                {
                    if (error != 0)
                    {
                        throw Log.ErrorAndCreateException<Win32Exception>($"Function {nameof(User32.DisplayConfigGetDeviceInfo)} returns error code '{error}'");
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

        private static string ConvertEDIDManufactureIdToCode(int manufactureEdid)
        {
            if (manufactureEdid == 0)
            {
                return string.Empty;
            }

            manufactureEdid = ((manufactureEdid & 0xff00) >> 8) | ((manufactureEdid & 0x00ff) << 8);
            var char1 = Convert.ToChar((byte)'A' + (manufactureEdid & 0x1f) - 1);
            var char2 = Convert.ToChar((byte)'A' + ((manufactureEdid >> 5) & 0x1f) - 1);
            var char3 = Convert.ToChar((byte)'A' + ((manufactureEdid >> 10) & 0x1f) - 1);

            return $"{char3}{char2}{char1}";
        }
    }
}
