namespace Orchestra.Theming
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    public class AccentColorService : IAccentColorService
    {
        private Color? _accentColor;

        public virtual Color GetAccentColor()
        {
            if (_accentColor.HasValue)
            {
                return _accentColor.Value;
            }

            var accentColorBrush = Application.Current.TryFindResource("AccentColorBrush") as SolidColorBrush;
            var finalBrush = accentColorBrush ?? Brushes.Orange;

            _accentColor = finalBrush.Color;

            return _accentColor.Value;
        }

        public virtual void SetAccentColor(Color color)
        {
            _accentColor = color;

            RaiseAccentColorChanged();
        }

        public event EventHandler<EventArgs> AccentColorChanged;

        protected void RaiseAccentColorChanged()
        {
            AccentColorChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
