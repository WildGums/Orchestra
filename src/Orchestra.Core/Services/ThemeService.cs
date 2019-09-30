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

        public ThemeService(Orc.Controls.Services.IAccentColorService accentColorService)
        {
            Argument.IsNotNull(() => accentColorService);

            _accentColorService = accentColorService;
        }

        public virtual bool ShouldCreateStyleForwarders()
        {
            return true;
        }

        public virtual ThemeInfo CreateThemeInfo()
        {
            var accentColor = _accentColorService.GetAccentColor();

            var themeInfo = new ThemeInfo
            {
                BaseColorScheme = "Light",
                AccentBaseColor = accentColor,
                HighlightColor = accentColor
            };

            return themeInfo;
        }
    }
}
