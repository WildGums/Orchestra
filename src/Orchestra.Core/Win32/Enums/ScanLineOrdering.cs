namespace Orchestra.Win32
{
    internal enum ScanLineOrdering : uint
    {
        DisplayConfigScanlineOrderingUnspecified = 0,
        DisplayConfigScanlineOrderingProgressive = 1,
        DisplayConfigScanlineOrderingInterlaced = 2,
        DisplayConfigScanlineOrderingInterlacedUpperFieldFirst = DisplayConfigScanlineOrderingInterlaced,
        DisplayConfigScanlineOrderingInterlacedLowerFieldFirst = 3,
        DisplayConfigScanlineOrderingForceUint32 = 0xFFFFFFFF
    }
}
