namespace Orchestra.Themes
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using Catel;
    using Catel.Logging;
    using MahApps.Metro;
    using Orc.Controls;
    using Orchestra.Services;

    public class MahAppsShellTheme : IShellTheme
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly Orc.Controls.Services.IAccentColorService _accentColorService;
        private readonly IThemeService _themeService;
        private readonly IBaseColorService _baseColorService;

        public MahAppsShellTheme(Orc.Controls.Services.IAccentColorService accentColorService,            
            IThemeService themeService,
            IBaseColorService baseColorService)
        {
            Argument.IsNotNull(() => accentColorService);
            Argument.IsNotNull(() => themeService);
            Argument.IsNotNull(() => baseColorService);

            _accentColorService = accentColorService;
            _themeService = themeService;
            _baseColorService = baseColorService;

            _accentColorService.AccentColorChanged += OnAccentBaseColorServiceAccentColorChanged;
            _baseColorService.BaseColorChanged += OnAccentBaseColorServiceAccentColorChanged;            
        }

        private void OnAccentBaseColorServiceAccentColorChanged(object sender, EventArgs e)
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

            MahAppsThemeHelper.CreateTheme(themeInfo.BaseColorScheme, themeInfo.AccentBaseColor, changeImmediately: true);

            // Note: important to add the resources dictionary *after* changing the app style, but then insert at the top 
            // so Fluent.Ribbon theme detection performance is best
            applicationResources.MergedDictionaries.Insert(1, resourceDictionary);
        }
    }
}
