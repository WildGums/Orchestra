// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Windows;
    using Catel;
    using Catel.IoC;
    using Catel.Reflection;
    using Orchestra.Theming;

    public static class ApplicationExtensions
    {
        public static void ApplyTheme(this Application application, bool createStyleForwarders = true)
        {
            Argument.IsNotNull(() => application);

            var serviceLocator = ServiceLocator.Default;
            var themeManager = serviceLocator.ResolveType<IThemeManager>();
            themeManager.EnsureApplicationThemes(application.GetType().GetAssemblyEx(), createStyleForwarders);
        }
    }
}
