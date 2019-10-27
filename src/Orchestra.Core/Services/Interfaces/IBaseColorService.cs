namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;

    public interface IBaseColorService
    {
        event EventHandler<EventArgs> BaseColorChanged;

        /// <summary>
        /// returns the available base colors (at least one)
        /// </summary>
        /// <returns>at least one base color, otherwise GetBaseColor will throw an exception</returns>
        IReadOnlyList<string> GetAvailableBaseColors();

        string GetBaseColor();
        bool SetBaseColor(string color);
    }
}
