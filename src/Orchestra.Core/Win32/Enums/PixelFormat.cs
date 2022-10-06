namespace Orchestra.Win32
{
    internal enum PixelFormat : uint
    {
        DisplayConfigPixelFormat8Bpp = 1,
        DisplayConfigPixelFormat16Bpp = 2,
        DisplayConfigPixelFormat24Bpp = 3,
        DisplayConfigPixelFormat32Bpp = 4,
        DisplayConfigPixelFormatNongdi = 5,
        DisplayConfigPixelFormatForceUint32 = 0xffffffff
    }
}
