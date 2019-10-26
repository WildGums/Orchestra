namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BaseColorService : IBaseColorService
    {
        public event EventHandler<EventArgs> BaseColorChanged;

        public string BaseColor = null;
        public string GetBaseColor() => BaseColor ?? (BaseColor = "Light");


        public void SetBaseColor(string color)
        {
            if (BaseColor == color)
                return;
            BaseColor = color;
            BaseColorChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
