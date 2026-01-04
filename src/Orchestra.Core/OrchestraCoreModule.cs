namespace Orchestra
{
    using Catel.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Orchestra.Changelog;
    using Orchestra.Collections;
    using Orchestra.Layers;
    using Orchestra;
    using Orchestra.Theming;
    using Orchestra.Tooltips;
    using Catel.IoC;
    using Orchestra.ViewModels;
    using Orchestra.Views;
    using Orchestra.Changelog.ViewModels;
    using Orchestra.Changelog.Views;
    using Catel.ThirdPartyNotices;

    /// <summary>
    /// Core module which allows the registration of default services in the service collection.
    /// </summary>
    public static class OrchestraCoreModule
    {
        public static IServiceCollection AddOrchestraCore(this IServiceCollection serviceCollection)
        {
            // Overrides of existing services
            serviceCollection.AddSingleton<IBusyIndicatorService, Orchestra.BusyIndicatorService>();
            serviceCollection.AddSingleton<ISelectDirectoryService, MicrosoftApiSelectDirectoryService>();
            serviceCollection.AddSingleton<IMessageService, Orchestra.MessageService>();

            // Regular services
            serviceCollection.TryAddSingleton<IChangelogService, ChangelogService>();
            serviceCollection.TryAddSingleton<IChangelogSnapshotService, ChangelogSnapshotService>();

            // Services
            serviceCollection.TryAddSingleton<IThirdPartyNoticesService, ThirdPartyNoticesService>();
            serviceCollection.TryAddSingleton<ICloseApplicationService, CloseApplicationService>();
            serviceCollection.TryAddSingleton<IRecentlyUsedItemsService, RecentlyUsedItemsService>();

            serviceCollection.TryAddSingleton<IKeyboardMappingsAllowedKeysService, KeyboardMappingsAllowedKeysService>();
            serviceCollection.TryAddSingleton<IKeyboardMappingsService, KeyboardMappingsService>();

            serviceCollection.TryAddSingleton<IStatusFilterService, StatusFilterService>();
            serviceCollection.TryAddSingleton<IStatusService, StatusService>();
            serviceCollection.TryAddSingleton<Orchestra.ISplashScreenService, Orchestra.SplashScreenService>();
            serviceCollection.TryAddSingleton<Orchestra.ISplashScreenStatusService, Orchestra.SplashScreenStatusService>();

            serviceCollection.TryAddSingleton<IMainWindowService, MainWindowService>();
            serviceCollection.TryAddSingleton<ICommandInfoService, CommandInfoService>();
            serviceCollection.TryAddSingleton<IManageAppDataService, ManageAppDataService>();
            serviceCollection.TryAddSingleton<IEnsureStartupService, EnsureStartupService>();
            serviceCollection.TryAddSingleton<IAboutInfoService, AboutInfoService>();
            serviceCollection.TryAddSingleton<IAboutService, AboutService>();
            serviceCollection.TryAddSingleton<IClipboardService, ClipboardService>();
            serviceCollection.TryAddSingleton<IViewActivationService, ViewActivationService>();
            serviceCollection.TryAddSingleton<IConfigurationBackupService, ConfigurationBackupService>();

            // Theming
            serviceCollection.TryAddSingleton<IThemeManager, ThemeManager>();

            // Hints system
            serviceCollection.TryAddSingleton<IAdorneredTooltipsCollection, AdorneredTooltipsCollection>();
            serviceCollection.TryAddSingleton<IAdornerLayer, HintsAdornerLayer>();
            serviceCollection.TryAddSingleton<IAdorneredTooltipsManager, AdorneredTooltipsManager>();
            serviceCollection.TryAddSingleton<IHintsProvider, HintsProvider>();

            serviceCollection.TryAddTransient<IAdornerLayer, HintsAdornerLayer>();
            serviceCollection.TryAddTransient<IAdorneredTooltipFactory, AdorneredTooltipFactory>();
            serviceCollection.TryAddTransient<IAdorneredTooltipsCollection, AdorneredTooltipsCollection>();

            // Custom views (sharing same view model)
            serviceCollection.TryAddSingleton<UIVisualizerInitializer>();

            serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new LibraryThirdPartyNotice("Orchestra", "https://github.com/wildgums/orchestra", "Orchestra.Core", "Orchestra"));
            serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new ResourceBasedThirdPartyNotice("ControlzEx", "https://github.com/ControlzEx/ControlzEx/", "Orchestra.Core", "Orchestra", "Resources.ThirdPartyNotices.controlzex.txt"));
            serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new ResourceBasedThirdPartyNotice("Newtonsoft.Json", "https://www.newtonsoft.com/json", "Orchestra.Core", "Orchestra", "Resources.ThirdPartyNotices.newtonsoft.json.txt"));

            serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orchestra.Core", "Orchestra.Properties", "Resources"));

            DotNetPatchHelper.Initialize();

            return serviceCollection;
        }

        private class UIVisualizerInitializer : IConstructAtStartup
        {
            public UIVisualizerInitializer(IUIVisualizerService uiVisualizerService)
            {
                uiVisualizerService.Register<KeyboardMappingsCustomizationViewModel, KeyboardMappingsCustomizationWindow>(false);
                uiVisualizerService.Register<KeyboardMappingsOverviewViewModel, KeyboardMappingsOverviewWindow>(false);
                uiVisualizerService.Register<ChangelogViewModel, ChangelogWindow>(false);
            }
        }
    }
}
