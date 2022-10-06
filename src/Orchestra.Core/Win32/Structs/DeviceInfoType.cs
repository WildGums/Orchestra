namespace Orchestra.Win32
{
    internal enum DeviceInfoType : uint
    {
        DisplayConfigDeviceInfoGetSourceName = 1,
        DisplayConfigDeviceInfoGetTargetName = 2,
        DisplayConfigDeviceInfoGetTargetPreferredMode = 3,
        DisplayConfigDeviceInfoGetAdapterName = 4,
        DisplayConfigDeviceInfoSetTargetPersistence = 5,
        DisplayConfigDeviceInfoGetTargetBaseType = 6,
        DisplayConfigDeviceInfoGetSupportVirtualResolution = 7,
        DisplayConfigDeviceInfoSetSupportVirtualResolution = 8,
        DisplayConfigDeviceInfoGetAdvancedColorInfo = 9,
        DisplayConfigDeviceInfoSetAdvancedColorState = 10,
        DisplayConfigDeviceInfoGetSdrWhiteLevel = 11,
        DisplayConfigDeviceInfoForceUint32 = 0xFFFFFFFF
    }
}
