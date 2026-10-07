namespace Orchestra;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using Catel.Logging;
using Catel.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orc.Theming;
using Orchestra.Changelog;
using Orchestra.Changelog.ViewModels;
using Orchestra.Theming;

public class ApplicationInitializationServiceBase : IApplicationInitializationService
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(ApplicationInitializationServiceBase));

    public ApplicationInitializationServiceBase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public virtual bool ShowSplashScreen => true;

    public virtual bool ShowShell => true;

    public virtual bool ShowChangelog => true;

    public IServiceProvider ServiceProvider { get; }

    public virtual async Task InitializeBeforeShowingSplashScreenAsync()
    {
        var xmlLanguage = GetApplicationLanguage();
        InitializeApplicationLanguage(xmlLanguage);

        // Note: we only have to create style forwarders once
        var xamlResourceService = ServiceProvider.GetRequiredService<IXamlResourceService>();
        var xamlResourceDictionaries = xamlResourceService.GetApplicationResourceDictionaries();

        var orchestraThemeManager = ServiceProvider.GetRequiredService<IThemeManager>();

        foreach (var xamlResourceDictionary in xamlResourceDictionaries)
        {
            orchestraThemeManager.EnsureApplicationThemes(xamlResourceDictionary, false);
        }

        var themeService = ServiceProvider.GetRequiredService<IThemeService>();
        if (themeService.ShouldCreateStyleForwarders())
        {
            StyleHelper.CreateStyleForwardersForDefaultStyles();
        }

        var orcThemingThemeManager = ServiceProvider.GetRequiredService<Orc.Theming.ThemeManager>();
        orcThemingThemeManager.SynchronizeTheme();
    }

    public virtual async Task InitializeBeforeCreatingShellAsync()
    {

    }

    public virtual async Task InitializeAfterCreatingShellAsync()
    {
    }

    public virtual async Task InitializeBeforeShowingShellAsync()
    {
    }

    public virtual async Task InitializeAfterShowingShellAsync()
    {
        if (ShowChangelog)
        {
            await ShowChangelogAsync();
        }
    }

    protected static async Task RunAndWaitAsync(params Func<Task>[] actions)
    {
        var tasks = new List<Task>();

        foreach (var action in actions)
        {
            tasks.Add(Task.Run(action));
        }

        await Task.WhenAll(tasks);
    }

    protected virtual CultureInfo GetApplicationCulture()
    {
        return CultureInfo.CurrentCulture;
    }

    protected virtual XmlLanguage GetApplicationLanguage()
    {
        var culture = GetApplicationCulture();
        var xmlLanguage = XmlLanguage.GetLanguage(culture.IetfLanguageTag);

        return xmlLanguage;
    }
    
    protected virtual void InitializeApplicationLanguage(XmlLanguage xmlLanguage)
    {
        ArgumentNullException.ThrowIfNull(xmlLanguage);

        Logger.LogDebug("Setting application language to '{Language}'", xmlLanguage.IetfLanguageTag);

        // Ensure that we are using the right culture
#pragma warning disable WPF0011 // Containing type should be used as registered owner.
        FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(xmlLanguage));
#pragma warning restore WPF0011 // Containing type should be used as registered owner.
    }

    protected virtual async Task ShowChangelogAsync()
    {
        var changelogService = ServiceProvider.GetRequiredService<IChangelogService>();

        var changelog = await changelogService.GetChangelogSinceSnapshotAsync();
        if (!changelog.IsEmpty)
        {
            var uiVisualizerService = ServiceProvider.GetRequiredService<IUIVisualizerService>();
            await uiVisualizerService.ShowDialogAsync<ChangelogViewModel>(changelog);
        }
    }
}
