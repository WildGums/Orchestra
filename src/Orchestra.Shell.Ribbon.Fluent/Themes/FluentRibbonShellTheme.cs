namespace Orchestra.Themes
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Xml;
    using Catel;
    using Catel.Logging;
    using Fluent;
    using Orchestra.Services;

    public class FluentRibbonShellTheme : IShellTheme
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly Orc.Controls.Services.IAccentColorService _accentColorService;
        private readonly IThemeService _themeService;

        public FluentRibbonShellTheme(Orc.Controls.Services.IAccentColorService accentColorService,
            IThemeService themeService)
        {
            Argument.IsNotNull(() => accentColorService);
            Argument.IsNotNull(() => themeService);

            _accentColorService = accentColorService;
            _themeService = themeService;

            _accentColorService.AccentColorChanged += OnAccentColorServiceAccentColorChanged;
        }

        private void OnAccentColorServiceAccentColorChanged(object sender, EventArgs e)
        {
            ApplyTheme(_themeService.GetThemeInfo());
        }

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
