namespace Orchestra.Theming
{
    using System.Reflection;
    using System.Windows;

    public interface IThemeManager
    {
        void EnsureApplicationThemes(Assembly assembly, bool createStyleForwarders = false);
        void EnsureApplicationThemes(string resourceDictionaryUri, bool createStyleForwarders = false);
        void EnsureApplicationThemes(ResourceDictionary resourceDictionary, bool createStyleForwarders = false);
        bool IsResourceDictionaryAvailable(string resourceDictionaryUri);
        void SynchronizeTheme();
    }
}
