namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Catel;

    public class BaseColorSchemeService : IBaseColorSchemeService
    {
        public event EventHandler<EventArgs> BaseColorSchemeChanged;

        private string _baseColorScheme = null;

        public string GetBaseColorScheme() => _baseColorScheme ?? (_baseColorScheme = GetAvailableBaseColorSchemes()[0]);

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
            return new List<string>() { OrchestraEnvironment.LightBaseColorScheme }.AsReadOnly();
        }
    }
}
