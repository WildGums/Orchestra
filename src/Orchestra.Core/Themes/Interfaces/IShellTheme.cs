namespace Orchestra.Themes
{
    using System.Windows;
    using System.Windows.Media;

    public interface IShellTheme
    {
        ResourceDictionary CreateResourceDictionary(ThemeInfo themeInfo);

        void ApplyTheme(ThemeInfo themeInfo);
    }
}
