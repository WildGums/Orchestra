namespace Orchestra.Win32
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
    internal struct NativeMonitorInfoEx : IEquatable<NativeMonitorInfoEx>
    {
        public uint Size;
        public RECT Monitor;
        public NativeRectangle Work;
        public int Flags;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public char[] Device;

        public static NativeMonitorInfoEx Initialize()
        {
            return new NativeMonitorInfoEx()
            {
                Size = (uint)Marshal.SizeOf(typeof(NativeMonitorInfoEx)),
                Monitor = new NativeRectangle(),
                Work = new NativeRectangle(),
                Flags = 0,
                Device = new char[32]
            };
        }

        public string GetDeviceName()
        {
            if (Device is null)
            {
                return null;
            }

            return new string(Device.Where(c => c != '\0')?.ToArray());
        }

        public bool Equals(NativeMonitorInfoEx other)
        {
            return Monitor.Equals(other.Monitor) && Work.Equals(other.Work) && Flags == other.Flags;
        }

        public override bool Equals(object obj)
        {
            return obj is NativeMonitorInfoEx m && m.Equals(this);
        }

        public override int GetHashCode()
        {
            return Device.GetHashCode() ^ Monitor.GetHashCode() ^ Work.GetHashCode();
        }

        public static bool operator ==(NativeMonitorInfoEx x, NativeMonitorInfoEx y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(NativeMonitorInfoEx x, NativeMonitorInfoEx y)
        {
            return !(x == y);
        }
    }
}
