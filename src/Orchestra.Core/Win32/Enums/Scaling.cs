namespace Orchestra.Win32
{
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
}
