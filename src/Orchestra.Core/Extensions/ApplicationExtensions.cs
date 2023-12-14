namespace Orchestra
{
    using System;
    using System.Windows;
    using Catel.IoC;
    using Catel.Reflection;
    using Orchestra.Theming;

    public static class ApplicationExtensions
    {
        public static void ApplyTheme(this Application application, bool createStyleForwarders = true)
        {
            ArgumentNullException.ThrowIfNull(application);

            var serviceLocator = ServiceLocator.Default;
            var themeManager = serviceLocator.ResolveRequiredType<IThemeManager>();
            themeManager.EnsureApplicationThemes(application.GetType().GetAssemblyEx(), createStyleForwarders);
        }
    }
}
