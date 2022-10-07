namespace Orchestra.Win32
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfig2DRegion
    {
        public uint Cx;
        public uint Cy;
    }
}
