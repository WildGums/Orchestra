namespace Orchestra.Win32
{
    using System;
    using System.Runtime.InteropServices;
    using Orchestra.Win32;
    using static Orchestra.Windows.MonitorInfo;

    internal static class User32
    {
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern int GetDisplayConfigBufferSizes(QueryDeviceConfigFlags flags, out uint numPathArrayElements, out uint numModeInfoArrayElements);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref NativeMonitorInfoEx info);

        [DllImport("user32.dll")]
        public static extern Win32Status DisplayConfigGetDeviceInfo(ref DisplayConfigAdapterName deviceName);

        [DllImport("user32.dll")]
        public static extern Win32Status DisplayConfigGetDeviceInfo(ref DisplayConfigTargetDeviceName deviceName);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool EnumDisplayDevices(string deviceName, uint deviceNumber, ref DisplayDevice displayDevice, uint flags);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

        public delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        [DllImport("user32.dll")]
        public static extern int QueryDisplayConfig([In] QueryDeviceConfigFlags flags, [In, Out] ref uint numPathArrayElements, [Out] DisplayConfigPathInfo[] pathInfoArray,
            [In, Out] ref uint numModeInfoArrayElements, [Out] DisplayConfigModeInfo[] modeInfoArray, IntPtr currentTopologyId);
    }
}
