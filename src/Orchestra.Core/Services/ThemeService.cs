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
        private readonly IBaseColorService _baseColorService;

        public ThemeService(Orc.Controls.Services.IAccentColorService accentColorService,IBaseColorService baseColorService)
        {
            Argument.IsNotNull(() => accentColorService);
            Argument.IsNotNull(() => baseColorService);

            _accentColorService = accentColorService;
            _baseColorService = baseColorService;
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
                BaseColorScheme = _baseColorService.GetBaseColor(),
                AccentBaseColor = accentColor,
                HighlightColor = accentColor
            };

            return themeInfo;
        }
    }
}
