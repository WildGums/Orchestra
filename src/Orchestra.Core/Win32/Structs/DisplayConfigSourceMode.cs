﻿namespace Orchestra.Win32
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigSourceMode
    {
        public uint Width;
        public uint Height;
        public PixelFormat PixelFormat;
        public PointL Position;
    }
}
