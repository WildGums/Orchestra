namespace Orchestra.Win32
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct DisplayConfigAdapterName
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        [MarshalAs(UnmanagedType.Struct)]
        private readonly DisplayConfigDeviceInfoHeader _header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public readonly string AdapterDevicePath;

        public DisplayConfigAdapterName(LUID adapter, DeviceInfoType deviceInfoType) : this()
        {
            _header = DisplayConfigDeviceInfoHeader.Initialize(adapter, deviceInfoType, (uint)Marshal.SizeOf<DisplayConfigAdapterName>());
        }
    }
}
