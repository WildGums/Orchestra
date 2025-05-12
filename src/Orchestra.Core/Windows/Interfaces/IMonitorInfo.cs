namespace Orchestra.Windows
{
    using System.Windows;

    public interface IMonitorInfo
    {
        string AdapterDeviceName { get; set; }
        string Availability { get; set; }
        string DeviceName { get; set; }
        string DeviceNameFull { get; set; }
        DpiScale DpiScale { get; set; }
        string FriendlyName { get; set; }
        string Id { get; set; }
        bool IsPrimary { get; set; }
        string ManufactureCode { get; set; }
        Int32Rect MonitorArea { get; set; }
        ushort ProductCodeId { get; set; }
        string ScreenHeight { get; set; }
        string ScreenWidth { get; set; }
        Int32Rect WorkingArea { get; set; }
    }
}
