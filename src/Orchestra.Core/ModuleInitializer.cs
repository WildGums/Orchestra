// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using Catel.Configuration;
using Catel.IoC;
using Catel.Logging;
using Catel.Reflection;
using Catel.Services;
using Orchestra;
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
        var serviceLocator = ServiceLocator.Default;

        // Ensure that we are using the right culture
#pragma warning disable WPF0011 // Containing type should be used as registered owner.
        FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
            new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
#pragma warning restore WPF0011 // Containing type should be used as registered owner.

        // Overide style of Catel please wait service
        serviceLocator.RegisterType<IPleaseWaitService, Orchestra.Services.PleaseWaitService>();

        // Override Catel.SelectDirectoryService with Orchestra.Services.SelectDirectoryService
        serviceLocator.RegisterType<ISelectDirectoryService, MicrosoftApiSelectDirectoryService>();

        serviceLocator.RegisterTypeIfNotYetRegistered<IThirdPartyNoticesService, ThirdPartyNoticesService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<ICloseApplicationService, CloseApplicationService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IRecentlyUsedItemsService, RecentlyUsedItemsService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IKeyboardMappingsService, KeyboardMappingsService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IStatusFilterService, StatusFilterService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IStatusService, StatusService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<Orchestra.Services.ISplashScreenService, Orchestra.Services.SplashScreenService>();

        serviceLocator.RegisterTypeIfNotYetRegistered<ICommandInfoService, CommandInfoService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IManageAppDataService, ManageAppDataService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IEnsureStartupService, EnsureStartupService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IAboutInfoService, AboutInfoService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IAboutService, AboutService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IClipboardService, ClipboardService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IViewActivationService, ViewActivationService>();
        serviceLocator.RegisterType<IMessageService, Orchestra.Services.MessageService>();

        // Theming
        serviceLocator.RegisterTypeIfNotYetRegistered<IAccentColorService, AccentColorService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IBaseColorSchemeService, BaseColorSchemeService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IThemeService, ThemeService>();
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
        var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();
        uiVisualizerService.Register<KeyboardMappingsCustomizationViewModel, KeyboardMappingsCustomizationWindow>(false);
        uiVisualizerService.Register<KeyboardMappingsOverviewViewModel, KeyboardMappingsOverviewWindow>(false);

        var thirdPartyNoticesService = serviceLocator.ResolveType<IThirdPartyNoticesService>();
        thirdPartyNoticesService.AddWithTryCatch(() => new ResourceBasedThirdPartyNotice("Catel", "https://www.catelproject.com", "Orchestra.Core", "Orchestra", "Resources.ThirdPartyNotices.catel.txt"));
        thirdPartyNoticesService.AddWithTryCatch(() => new ResourceBasedThirdPartyNotice("ControlzEx", "https://github.com/ControlzEx/ControlzEx/", "ControlzEx", "ControlzEx", "Resources.ThirdPartyNotices.controlzex.txt"));
        thirdPartyNoticesService.AddWithTryCatch(() => new ResourceBasedThirdPartyNotice("DotNetZip", string.Empty, "Orchestra.Core", "Orchestra", "Resources.ThirdPartyNotices.dotnetzip.txt"));
        thirdPartyNoticesService.AddWithTryCatch(() => new ResourceBasedThirdPartyNotice("Orchestra", "https://opensource.wildgums.com", "Orchestra.Core", "Orchestra", "Resources.ThirdPartyNotices.orchestra.txt"));
        thirdPartyNoticesService.AddWithTryCatch(() => new ResourceBasedThirdPartyNotice("Ricciolo", string.Empty, "Orchestra.Core", "Orchestra", "Resources.ThirdPartyNotices.ricciolo.txt"));

        var languageService = serviceLocator.ResolveType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orchestra.Core", "Orchestra.Properties", "Resources"));

        DotNetPatchHelper.Initialize();
    }
}
