namespace Orchestra.Services
{
    using System.Windows.Media;

    [ObsoleteEx(TreatAsErrorFromVersion = "5.2", RemoveInVersion = "6.0", ReplacementTypeOrMember = "Orc.Controls.Services.IAccentColorService")]
    public interface IAccentColorService
    {
        Color GetAccentColor();
    }
}
