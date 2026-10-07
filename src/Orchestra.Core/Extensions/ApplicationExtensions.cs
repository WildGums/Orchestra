namespace Orchestra;

using System;
using System.Windows;
using Catel.IoC;
using Catel.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Orchestra.Theming;

public static class ApplicationExtensions
{
    public static void ApplyTheme(this Application application, bool createStyleForwarders = true)
    {
        ArgumentNullException.ThrowIfNull(application);

        var serviceProvider = IoCContainer.ServiceProvider;
        var themeManager = serviceProvider.GetRequiredService<IThemeManager>();
        themeManager.EnsureApplicationThemes(application.GetType().GetAssemblyEx(), createStyleForwarders);
    }
}
