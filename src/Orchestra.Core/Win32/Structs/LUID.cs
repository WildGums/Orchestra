namespace Orchestra.Win32
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct LUID
    {
        public uint LowPart;
        public int HighPart;

        public override string ToString()
        {
            return string.Format("{0}{1}", LowPart, HighPart);
        }
    }
}
