namespace Orchestra.Themes
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Xml;
    using Catel.Logging;
    using Fluent;

    public class FluentRibbonShellTheme : IShellTheme
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public ResourceDictionary CreateResourceDictionary(ThemeInfo themeInfo)
        {
            // Not required
            return null;
        }

        public void ApplyTheme(ThemeInfo themeInfo)
        {
            var application = Application.Current;
            var applicationResources = application.Resources;
            var resourceDictionary = Orchestra.ThemeHelper.GetAccentColorResourceDictionary();

            FluentRibbonThemeHelper.CreateTheme(themeInfo.BaseColorScheme, themeInfo.AccentBaseColor, themeInfo.HighlightColor, changeImmediately: true);

            // Note: important to add the resources dictionary *after* changing the app style, but then insert at the top 
            // so Fluent.Ribbon theme detection performance is best
            applicationResources.MergedDictionaries.Insert(1, resourceDictionary);
        }
    }
}
