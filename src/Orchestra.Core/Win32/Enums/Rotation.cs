namespace Orchestra.Win32
{
    internal enum Rotation : uint
    {
        DisplayConfigRotationIdentity = 1,
        DisplayConfigRotationRotate90 = 2,
        DisplayConfigRotationRotate180 = 3,
        DisplayConfigRotationRotate270 = 4,
        DisplayConfigRotationForceUint32 = 0xFFFFFFFF
    }
}
