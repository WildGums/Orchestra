// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Services
{
    using Catel;

    public class ThemeService : IThemeService
    {
        private readonly Orc.Controls.Services.IAccentColorService _accentColorService;
        private readonly IBaseColorSchemeService _baseColorSchemeService;

        public ThemeService(Orc.Controls.Services.IAccentColorService accentColorService,IBaseColorSchemeService baseColorSchemeService)
        {
            Argument.IsNotNull(() => accentColorService);
            Argument.IsNotNull(() => baseColorSchemeService);

            _accentColorService = accentColorService;
            _baseColorSchemeService = baseColorSchemeService;
        }

        public virtual bool ShouldCreateStyleForwarders()
        {
            return true;
        }

        public virtual ThemeInfo GetThemeInfo()
        {
            var accentColor = _accentColorService.GetAccentColor();

            var themeInfo = new ThemeInfo
            {
                BaseColorScheme = _baseColorSchemeService.GetBaseColorScheme(),
                AccentBaseColor = accentColor,
                HighlightColor = accentColor
            };

            return themeInfo;
        }
    }
}
