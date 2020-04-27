namespace Orchestra.Theming
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel;

    public class BaseColorSchemeService : IBaseColorSchemeService
    {
        private readonly ControlzEx.Theming.ThemeManager _themeManager;

        private string _baseColorScheme = null;

        public BaseColorSchemeService()
        {
            _themeManager = ControlzEx.Theming.ThemeManager.Current;
        }

        public string GetBaseColorScheme() => _baseColorScheme ?? (_baseColorScheme = GetAvailableBaseColorSchemes()[0]);

        public event EventHandler<EventArgs> BaseColorSchemeChanged;

        public bool SetBaseColorScheme(string color)
        {
            if (_baseColorScheme.EqualsIgnoreCase(color) || !GetAvailableBaseColorSchemes().Contains(color))
            { 
                return false; 
            }

            _baseColorScheme = color;

            BaseColorSchemeChanged?.Invoke(this, EventArgs.Empty);

            return true;
        }

        public virtual IReadOnlyList<string> GetAvailableBaseColorSchemes()
        {
            var baseColors = _themeManager.BaseColors;
            if (baseColors.Count > 0)
            {
                return baseColors;
            }

            return new[] { "Light", "Dark" }; 
        }
    }
}
