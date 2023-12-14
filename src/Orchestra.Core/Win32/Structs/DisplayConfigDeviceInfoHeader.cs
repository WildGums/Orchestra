namespace Orchestra.Win32
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigDeviceInfoHeader
    {
        public DeviceInfoType Type;
        public uint Size;
        public LUID AdapterId;
        public uint Id;

        public static DisplayConfigDeviceInfoHeader Initialize(LUID adapterId, DeviceInfoType requestType, uint size)
        {
            return new DisplayConfigDeviceInfoHeader()
            {
                AdapterId = adapterId,
                Type = requestType,
                Size = size
            };
        }

        public static DisplayConfigDeviceInfoHeader Initialize(LUID adapterId, uint targetId, DeviceInfoType requestType, uint size)
        {
            return new DisplayConfigDeviceInfoHeader()
            {
                AdapterId = adapterId,
                Id = targetId,
                Type = requestType,
                Size = size
            };
        }
    }
}
