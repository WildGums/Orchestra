namespace Orchestra.Examples.MahApps.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Orc.Theming;

    public class MaterialDesignThemeService : Orc.Theming.ThemeService
    {
        public MaterialDesignThemeService(IAccentColorService accentColorService, IBaseColorSchemeService baseColorSchemeService) 
            : base(accentColorService, baseColorSchemeService)
        {
        }

        public override bool ShouldCreateStyleForwarders()
        {
            return false;
        }
    }
}
