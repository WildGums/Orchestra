namespace Orchestra.Services
{
    using System;
    using System.Windows.Media;

    public interface IBaseColorService
    {
        event EventHandler<EventArgs> BaseColorChanged;

        string GetBaseColor();
        void SetBaseColor(string color);
    }
}
