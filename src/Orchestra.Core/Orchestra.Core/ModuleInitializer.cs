// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Catel.IoC;
using Catel.Services;
using Catel.Services.Models;
using Orchestra;
using Orchestra.Collections;
using Orchestra.Layers;
using Orchestra.Services;
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

        serviceLocator.RegisterTypeIfNotYetRegistered<IRecentlyUsedItemsService, RecentlyUsedItemsesService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IKeyboardMappingsService, KeyboardMappingsService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IStatusService, StatusService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<Orchestra.Services.ISplashScreenService, Orchestra.Services.SplashScreenService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IAboutService, AboutService>();

        // Hints system
        serviceLocator.RegisterType<IAdorneredTooltipsCollection, AdorneredTooltipsCollection>();
        serviceLocator.RegisterType<IAdornerLayer, HintsAdornerLayer>();
        serviceLocator.RegisterType<IAdorneredTooltipsManager, AdorneredTooltipsManager>();
        serviceLocator.RegisterType<IHintsProvider, HintsProvider>();

        serviceLocator.RegisterType<IAdornerLayer, HintsAdornerLayer>(RegistrationType.Transient);
        serviceLocator.RegisterType<IAdorneredTooltipFactory, AdorneredTooltipFactory>(RegistrationType.Transient);
        serviceLocator.RegisterType<IAdorneredTooltipsCollection, AdorneredTooltipsCollection>(RegistrationType.Transient);

        var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();

        // Can be replaced by this in the future:
        //uiVisualizerService.Register<KeyboardMappingsCustomizationViewModel, KeyboardMappingsCustomizationWindow>(false);
        //uiVisualizerService.Register<KeyboardMappingsOverviewViewModel, KeyboardMappingsOverviewWindow>(false);

        if (!uiVisualizerService.IsRegistered(typeof (KeyboardMappingsCustomizationViewModel)))
        {
            uiVisualizerService.Register(typeof(KeyboardMappingsCustomizationViewModel), typeof(KeyboardMappingsCustomizationWindow), false);    
        }

        if (!uiVisualizerService.IsRegistered(typeof(KeyboardMappingsOverviewViewModel)))
        {
            uiVisualizerService.Register(typeof(KeyboardMappingsOverviewViewModel), typeof(KeyboardMappingsOverviewWindow), false);
        }

        var languageService = serviceLocator.ResolveType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orchestra", "Orchestra.Properties", "Resources"));
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orchestra.Core", "Orchestra.Properties", "Resources"));

        DotNetPatchHelper.Initialize();
    }
}