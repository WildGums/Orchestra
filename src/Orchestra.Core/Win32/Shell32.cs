namespace Orchestra.Win32
{
    using System;
    using System.Runtime.InteropServices;

    internal static class Shell32
    {
        [DllImport("shell32.dll", SetLastError = true)]
        internal static extern IntPtr SHAppBarMessage(ABM dwMessage, [In] ref APPBARDATA pData);
    }
}
