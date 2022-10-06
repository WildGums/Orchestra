namespace Orchestra.Win32
{
    using System.Runtime.InteropServices;
    using Orchestra.Win32.Enums;

    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigTargetMode
    {
        public DisplayConfigVideoSignalInfo TargetVideoSignalInfo;
    }
}
