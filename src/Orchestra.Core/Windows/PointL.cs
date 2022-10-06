namespace Orchestra.Windows
{
    using System.Runtime.InteropServices;

    public partial class MonitorInfo
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct PointL
        {
            private readonly int _x;
            private readonly int _y;
        }
    }
}
