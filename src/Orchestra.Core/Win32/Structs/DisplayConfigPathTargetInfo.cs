namespace Orchestra.Win32
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigPathTargetInfo
    {
        public LUID AdapterId;
        public uint Id;
        public uint ModeInfoIdx;
        private readonly VideoOutputTechnology _outputTechnology;
        private readonly Rotation _rotation;
        private readonly Scaling _scaling;
        private readonly DisplayConfigRational _refreshRate;
        private readonly ScanLineOrdering _scanLineOrdering;
        public bool TargetAvailable;
        public uint StatusFlags;
    }
}
