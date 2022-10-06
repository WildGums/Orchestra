namespace Orchestra.Win32
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;

    internal static class Psapi
    {
        [DllImport("psapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int GetMappedFileName(IntPtr hProcess, IntPtr lpv, StringBuilder lpFilename, int nSize);
    }
}
