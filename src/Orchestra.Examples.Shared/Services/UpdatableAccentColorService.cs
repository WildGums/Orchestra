namespace Orchestra.Examples.Services
{
    using System.Windows.Media;
    using Orc.Controls.Services;

    public class UpdatableAccentColorService : AccentColorService, IUpdatableAccentColorService
    {
        private Color? _accentColor;

        public UpdatableAccentColorService()
        {

        }

        public override Color GetAccentColor()
        {
            if (!_accentColor.HasValue)
            {
                _accentColor = base.GetAccentColor();
            }

            return _accentColor.Value;
        }

        public void SetAccentColor(Color color)
        {
            _accentColor = color;

            RaiseAccentColorChanged();
        }
    }
}
