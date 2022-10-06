namespace Orchestra.Win32
{
    using System.Runtime.InteropServices;
    using System.Windows;

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    internal struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public int GetWidth()
        {
            return right - left;
        }

        public int GetHeight()
        {
            return bottom - top;
        }

        public Int32Rect ToInt32Rect()
        {
            return new Int32Rect
            {
                X = left,
                Y = top,
                Width = GetWidth(),
                Height = GetHeight()
            };
        }

        public override string ToString()
        {
            return $"Left:{left} Top:{top} Right:{right} Bottom:{bottom}";
        }
    }
}
