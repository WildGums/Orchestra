namespace Orchestra
{
    using Catel.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Orchestra.Changelog;
    using Orchestra.Collections;
    using Orchestra.Layers;
    using Orchestra.Services;
    using Orchestra.Theming;
    using Orchestra.Tooltips;

    /// <summary>
    /// Core module which allows the registration of default services in the service collection.
    /// </summary>
    public static class OrchestraCoreModule
    {
        public static IServiceCollection AddOrcFileSystemServices(this IServiceCollection serviceCollection)
        {
            // Overrides of existing services
            serviceCollection.AddSingleton<IBusyIndicatorService, Orchestra.Services.BusyIndicatorService>();
            serviceCollection.AddSingleton<ISelectDirectoryService, MicrosoftApiSelectDirectoryService>();
            serviceCollection.AddSingleton<IMessageService, Orchestra.Services.MessageService>();

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
            serviceCollection.TryAddSingleton<Orchestra.Services.ISplashScreenService, Orchestra.Services.SplashScreenService>();
            serviceCollection.TryAddSingleton<Orchestra.Services.ISplashScreenStatusService, Orchestra.Services.SplashScreenStatusService>();

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

            //// Custom views (sharing same view model)
            //var uiVisualizerService = serviceLocator.ResolveRequiredType<IUIVisualizerService>();
            //uiVisualizerService.Register<KeyboardMappingsCustomizationViewModel, KeyboardMappingsCustomizationWindow>(false);
            //uiVisualizerService.Register<KeyboardMappingsOverviewViewModel, KeyboardMappingsOverviewWindow>(false);
            //uiVisualizerService.Register<ChangelogViewModel, ChangelogWindow>(false);

            serviceCollection.AddSingleton<ThirdPartyNotice>((x) => new ResourceBasedThirdPartyNotice("Catel", "https://www.catelproject.com", "Orchestra.Core", "Orchestra", "Resources.ThirdPartyNotices.catel.txt"));
            serviceCollection.AddSingleton<ThirdPartyNotice>((x) => new ResourceBasedThirdPartyNotice("ControlzEx", "https://github.com/ControlzEx/ControlzEx/", "Orchestra.Core", "Orchestra", "Resources.ThirdPartyNotices.controlzex.txt"));
            serviceCollection.AddSingleton<ThirdPartyNotice>((x) => new ResourceBasedThirdPartyNotice("Material Design Icons", "https://github.com/Templarian/MaterialDesign", "Orchestra.Core", "Orchestra", "Resources.ThirdPartyNotices.materialdesignicons.txt"));
            serviceCollection.AddSingleton<ThirdPartyNotice>((x) => new ResourceBasedThirdPartyNotice("Newtonsoft.Json", "https://github.com/JamesNK/Newtonsoft.Json", "Orchestra.Core", "Orchestra", "Resources.ThirdPartyNotices.newtonsoft.json.txt"));
            serviceCollection.AddSingleton<ThirdPartyNotice>((x) => new ResourceBasedThirdPartyNotice("Orchestra", "https://opensource.wildgums.com", "Orchestra.Core", "Orchestra", "Resources.ThirdPartyNotices.orchestra.txt"));

            serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orchestra.Core", "Orchestra.Properties", "Resources"));

            DotNetPatchHelper.Initialize();

            return serviceCollection;
        }
    }
}
