// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Catel.IoC;
using Catel.Services;
using Catel.Services.Models;
using Orchestra;
using Orchestra.Services;
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

        var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();
        uiVisualizerService.Register(typeof(KeyboardMappingsCustomizationViewModel), typeof(KeyboardMappingsCustomizationWindow));
        uiVisualizerService.Register(typeof(KeyboardMappingsOverviewViewModel), typeof(KeyboardMappingsOverviewWindow));

        var languageService = serviceLocator.ResolveType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orchestra", "Orchestra.Properties", "Resources"));

        DotNetPatchHelper.Initialize();
    }
}