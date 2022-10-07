namespace Orchestra.Win32
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    internal struct DisplayConfigModeInfoUnion
    {
        [FieldOffset(0)]
        public DisplayConfigTargetMode TargetMode;
        [FieldOffset(0)]
        public DisplayConfigSourceMode SourceMode;
    }
}
