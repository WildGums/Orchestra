namespace Orchestra.Theming
{
    public interface IThemeService
    {
        bool ShouldCreateStyleForwarders();
        ThemeInfo GetThemeInfo();
    }
}
