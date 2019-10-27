namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Catel;

    public class BaseColorService : IBaseColorService
    {
        public event EventHandler<EventArgs> BaseColorChanged;
        
        private string _baseColor = null;
        public string GetBaseColor() => _baseColor ?? (_baseColor = GetAvailableBaseColors()[0]);
        public bool SetBaseColor(string color)
        {
            if (_baseColor == color || !GetAvailableBaseColors().Contains(color))
                return false;
            _baseColor = color;
            BaseColorChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
       
        public virtual IReadOnlyList<string> GetAvailableBaseColors()
        {
            return new List<string>() { OrchestraEnvironment.DefaultBaseColor }.AsReadOnly();
        }
    }
}
