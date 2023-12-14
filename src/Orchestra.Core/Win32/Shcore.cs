namespace Orchestra.Win32
{
    using System;
    using System.Runtime.InteropServices;
    using static Orchestra.Windows.MonitorInfo;

    internal static class Shcore
    {
        [DllImport("shcore.dll")]
        public static extern bool GetDpiForMonitor([In] IntPtr hMonitor, [In] DpiType dpiType, [Out] out uint dpiX, [Out] out uint dpiY);

        [DllImport("shcore.dll", SetLastError = true)]
        public static extern void GetProcessDpiAwareness(IntPtr hProcess, out DpiAwareness awareness);

        [DllImport("shcore.dll", SetLastError = true)]
        public static extern bool SetProcessDpiAwareness(DpiAwareness awareness);
    }
}
