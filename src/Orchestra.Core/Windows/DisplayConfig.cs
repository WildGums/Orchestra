namespace Orchestra.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class DisplayConfig
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

        internal enum ModeInfoType : uint
        {
            DisplayConfigModeInfoTypeSource = 1,
            DisplayConfigModeInfoTypeTarget = 2,
            DisplayConfigModeInfoTypeForceUint32 = 0xFFFFFFFF
        }

        internal enum PixelFormat : uint
        {
            DisplayConfigPixelFormat8Bpp = 1,
            DisplayConfigPixelFormat16Bpp = 2,
            DisplayConfigPixelFormat24Bpp = 3,
            DisplayConfigPixelFormat32Bpp = 4,
            DisplayConfigPixelFormatNongdi = 5,
            DisplayConfigPixelFormatForceUint32 = 0xffffffff
        }

        internal enum Rotation : uint
        {
            DisplayConfigRotationIdentity = 1,
            DisplayConfigRotationRotate90 = 2,
            DisplayConfigRotationRotate180 = 3,
            DisplayConfigRotationRotate270 = 4,
            DisplayConfigRotationForceUint32 = 0xFFFFFFFF
        }

        internal enum Scaling : uint
        {
            DisplayСonfigScalingIdentity = 1,
            DisplayСonfigScalingCentered = 2,
            DisplayСonfigScalingStretched = 3,
            DisplayConfigScalingAspectRatioCenteredMax = 4,
            DisplayConfigScalingCustom = 5,
            DisplayConfigScalingPreferred = 128,
            DisplayConfigScalingForceUint32 = 0xFFFFFFFF
        }

        internal enum ScanLineOrdering : uint
        {
            DisplayConfigScanlineOrderingUnspecified = 0,
            DisplayConfigScanlineOrderingProgressive = 1,
            DisplayConfigScanlineOrderingInterlaced = 2,
            DisplayConfigScanlineOrderingInterlacedUpperFieldFirst = DisplayConfigScanlineOrderingInterlaced,
            DisplayConfigScanlineOrderingInterlacedLowerFieldFirst = 3,
            DisplayConfigScanlineOrderingForceUint32 = 0xFFFFFFFF
        }

        internal enum VideoOutputTechnology : uint
        {
            DisplayConfigOutputTechnologyOther = 0xFFFFFFFF,
            DisplayConfigOutputTechnologyHd15 = 0,
            DisplayConfigOutputTechnologySvideo = 1,
            DisplayConfigOutputTechnologyCompositeVideo = 2,
            DisplayConfigOutputTechnologyComponentVideo = 3,
            DisplayConfigOutputTechnologyDvi = 4,
            DisplayConfigOutputTechnologyHdmi = 5,
            DisplayConfigOutputTechnologyLvds = 6,
            DisplayConfigOutputTechnologyDJpn = 8,
            DisplayConfigOutputTechnologySdi = 9,
            DisplayconfigOutputTechnologyDisplayportExternal = 10,
            DisplayConfigOutputTechnologyDisplayportEmbedded = 11,
            DisplayConfigOutputTechnologyUdiExternal = 12,
            DisplayConfigOutputTechnologyUdiEmbedded = 13,
            DisplayConfigOutputTechnologySdtvdongle = 14,
            DisplayConfigOutputTechnologyMiracast = 15,
            DisplayConfigOutputTechnologyInternal = 0x80000000,
            DisplayConfigOutputTechnologyForceUint32 = 0xFFFFFFFF
        }        
    }
}
