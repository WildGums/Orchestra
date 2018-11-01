namespace Orchestra.Services
{
    using System.Windows;
    using System.Windows.Media;

    [ObsoleteEx(TreatAsErrorFromVersion = "5.2", RemoveInVersion = "6.0", ReplacementTypeOrMember = "Orc.Controls.Services.AccentColorService")]
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
