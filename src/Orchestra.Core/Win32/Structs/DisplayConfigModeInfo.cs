namespace Orchestra.Win32
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    internal struct DisplayConfigModeInfo
    {
        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(0)]
        public readonly DisplayConfig.ModeInfoType InfoType;

        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(4)]
        public readonly uint Id;

        [MarshalAs(UnmanagedType.Struct)]
        [FieldOffset(8)]
        public readonly LUID AdapterId;

        [MarshalAs(UnmanagedType.Struct)]
        [FieldOffset(16)]
        public readonly DisplayConfigModeInfoUnion TargetMode;
    }
}
