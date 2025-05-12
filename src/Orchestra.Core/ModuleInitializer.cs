﻿using Catel;
using Catel.IoC;
using Catel.Services;
using Orchestra;
using Orchestra.Changelog;
using Orchestra.Changelog.ViewModels;
using Orchestra.Changelog.Views;
using Orchestra.Collections;
using Orchestra.Layers;
using Orchestra.Services;
using Orchestra.Theming;
using Orchestra.Tooltips;
using Orchestra.ViewModels;
using Orchestra.Views;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        if (EnvironmentHelper.IsProcessHostedByTool)
        {
            return;
        }

        var serviceLocator = ServiceLocator.Default;

        // Overide style of Catel please wait service
        serviceLocator.RegisterType<IBusyIndicatorService, Orchestra.Services.BusyIndicatorService>();

        // Override Catel.SelectDirectoryService with Orchestra.Services.SelectDirectoryService
        serviceLocator.RegisterType<ISelectDirectoryService, MicrosoftApiSelectDirectoryService>();

        // Changelog
        serviceLocator.RegisterType<IChangelogService, ChangelogService>();
        serviceLocator.RegisterType<IChangelogSnapshotService, ChangelogSnapshotService>();

        // Services
        serviceLocator.RegisterTypeIfNotYetRegistered<IThirdPartyNoticesService, ThirdPartyNoticesService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<ICloseApplicationService, CloseApplicationService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IRecentlyUsedItemsService, RecentlyUsedItemsService>();

        serviceLocator.RegisterTypeIfNotYetRegistered<IKeyboardMappingsAllowedKeysService, KeyboardMappingsAllowedKeysService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IKeyboardMappingsService, KeyboardMappingsService>();

        serviceLocator.RegisterTypeIfNotYetRegistered<IStatusFilterService, StatusFilterService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IStatusService, StatusService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<Orchestra.Services.ISplashScreenService, Orchestra.Services.SplashScreenService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<Orchestra.Services.ISplashScreenStatusService, Orchestra.Services.SplashScreenStatusService>();

        serviceLocator.RegisterTypeIfNotYetRegistered<IMainWindowService, MainWindowService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<ICommandInfoService, CommandInfoService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IManageAppDataService, ManageAppDataService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IEnsureStartupService, EnsureStartupService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IAboutInfoService, AboutInfoService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IAboutService, AboutService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IClipboardService, ClipboardService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IViewActivationService, ViewActivationService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IConfigurationBackupService, ConfigurationBackupService>();
        serviceLocator.RegisterType<IMessageService, Orchestra.Services.MessageService>();

        // Theming
        serviceLocator.RegisterTypeIfNotYetRegistered<IThemeManager, ThemeManager>();

        // Hints system
        serviceLocator.RegisterType<IAdorneredTooltipsCollection, AdorneredTooltipsCollection>();
        serviceLocator.RegisterType<IAdornerLayer, HintsAdornerLayer>();
        serviceLocator.RegisterType<IAdorneredTooltipsManager, AdorneredTooltipsManager>();
        serviceLocator.RegisterType<IHintsProvider, HintsProvider>();

        serviceLocator.RegisterType<IAdornerLayer, HintsAdornerLayer>(RegistrationType.Transient);
        serviceLocator.RegisterType<IAdorneredTooltipFactory, AdorneredTooltipFactory>(RegistrationType.Transient);
        serviceLocator.RegisterType<IAdorneredTooltipsCollection, AdorneredTooltipsCollection>(RegistrationType.Transient);

        // Custom views (sharing same view model)
        var uiVisualizerService = serviceLocator.ResolveRequiredType<IUIVisualizerService>();
        uiVisualizerService.Register<KeyboardMappingsCustomizationViewModel, KeyboardMappingsCustomizationWindow>(false);
        uiVisualizerService.Register<KeyboardMappingsOverviewViewModel, KeyboardMappingsOverviewWindow>(false);
        uiVisualizerService.Register<ChangelogViewModel, ChangelogWindow>(false);

        var thirdPartyNoticesService = serviceLocator.ResolveRequiredType<IThirdPartyNoticesService>();
        thirdPartyNoticesService.AddWithTryCatch(() => new ResourceBasedThirdPartyNotice("Catel", "https://www.catelproject.com", "Orchestra.Core", "Orchestra", "Resources.ThirdPartyNotices.catel.txt"));
        thirdPartyNoticesService.AddWithTryCatch(() => new ResourceBasedThirdPartyNotice("ControlzEx", "https://github.com/ControlzEx/ControlzEx/", "Orchestra.Core", "Orchestra", "Resources.ThirdPartyNotices.controlzex.txt"));
        thirdPartyNoticesService.AddWithTryCatch(() => new ResourceBasedThirdPartyNotice("Material Design Icons", "https://github.com/Templarian/MaterialDesign", "Orchestra.Core", "Orchestra", "Resources.ThirdPartyNotices.materialdesignicons.txt"));
        thirdPartyNoticesService.AddWithTryCatch(() => new ResourceBasedThirdPartyNotice("Newtonsoft.Json", "https://github.com/JamesNK/Newtonsoft.Json", "Orchestra.Core", "Orchestra", "Resources.ThirdPartyNotices.newtonsoft.json.txt"));
        thirdPartyNoticesService.AddWithTryCatch(() => new ResourceBasedThirdPartyNotice("Orchestra", "https://opensource.wildgums.com", "Orchestra.Core", "Orchestra", "Resources.ThirdPartyNotices.orchestra.txt"));

        var languageService = serviceLocator.ResolveRequiredType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orchestra.Core", "Orchestra.Properties", "Resources"));

        DotNetPatchHelper.Initialize();
    }
}
