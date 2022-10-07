namespace Orchestra.Win32
{
    using System;

    [Flags]
    internal enum QueryDeviceConfigFlags : uint
    {
        QdcAllPaths = 0x00000001,
        QdcOnlyActivePaths = 0x00000002,
        QdcDatabaseCurrent = 0x00000004
    }
}
