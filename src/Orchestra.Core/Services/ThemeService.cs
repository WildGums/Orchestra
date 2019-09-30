// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    public class ThemeService : IThemeService
    {
        public virtual bool ShouldCreateStyleForwarders()
        {
            return true;
        }

        public virtual ThemeInfo CreateThemeInfo()
        {
            var accentColor = ThemeHelper.GetAccentColor();

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
