namespace Orchestra.Win32
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct PointL
    {
        private readonly int _x;
        private readonly int _y;
    }
}
