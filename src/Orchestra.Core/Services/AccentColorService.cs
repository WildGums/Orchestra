namespace Orchestra.Services
{
    using System.Windows;
    using System.Windows.Media;

    public class AccentColorService : IAccentColorService
    {
        public virtual Color GetAccentColor()
        {
            var accentColorBrush = Application.Current.TryFindResource("AccentColorBrush") as SolidColorBrush;
            var finalBrush = accentColorBrush ?? OrchestraEnvironment.DefaultAccentColorBrush;
            return finalBrush.Color;
        }
    }
}
