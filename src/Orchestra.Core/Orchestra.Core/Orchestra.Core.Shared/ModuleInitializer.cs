// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.IO;
using Catel.IoC;
using Catel.Logging;
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
        InitializeLogging();

        var serviceLocator = ServiceLocator.Default;

        // Overide style of Catel please wait service
        serviceLocator.RegisterType<IPleaseWaitService, Orchestra.Services.PleaseWaitService>();

        serviceLocator.RegisterTypeIfNotYetRegistered<ICloseApplicationService, CloseApplicationService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IRecentlyUsedItemsService, RecentlyUsedItemsService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IKeyboardMappingsService, KeyboardMappingsService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IStatusFilterService, StatusFilterService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IStatusService, StatusService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<Orchestra.Services.ISplashScreenService, Orchestra.Services.SplashScreenService>();

        serviceLocator.RegisterTypeIfNotYetRegistered<ICommandInfoService, CommandInfoService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IAppDataService, AppDataService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IEnsureStartupService, EnsureStartupService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IAboutInfoService, AboutInfoService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IAboutService, AboutService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IClipboardService, ClipboardService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IThemeService, ThemeService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IViewActivationService, ViewActivationService>();

        serviceLocator.RegisterType<IMessageService, Orchestra.Services.MessageService>();

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

        if (!uiVisualizerService.IsRegistered(typeof(KeyboardMappingsCustomizationViewModel)))
        {
            uiVisualizerService.Register(typeof(KeyboardMappingsCustomizationViewModel).FullName, typeof(KeyboardMappingsCustomizationWindow));
        }

        if (!uiVisualizerService.IsRegistered(typeof(KeyboardMappingsOverviewViewModel)))
        {
            uiVisualizerService.Register(typeof(KeyboardMappingsOverviewViewModel).FullName, typeof(KeyboardMappingsOverviewWindow));
        }

        var languageService = serviceLocator.ResolveType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orchestra.Core", "Orchestra.Properties", "Resources"));

        DotNetPatchHelper.Initialize();
    }

    private static void InitializeLogging()
    {
        // Delete all log files older than 1 week
        try
        {
            var applicationDataDirectory = Catel.IO.Path.GetApplicationDataDirectory();
            if (Directory.Exists(applicationDataDirectory))
            {
                var logFiles = Directory.GetFiles(applicationDataDirectory, "*.log");
                foreach (var logFile in logFiles)
                {
                    var lastWriteTime = File.GetLastWriteTime(logFile);
                    if (lastWriteTime < DateTime.Now.AddDays(-7))
                    {
                        File.Delete(logFile);
                    }
                }
            }
        }
        catch (Exception)
        {
            // Ignore
        }

        var fileLogListener = LogHelper.CreateFileLogListener(AssemblyHelper.GetEntryAssembly().GetName().Name);

        fileLogListener.IgnoreCatelLogging = true;
        fileLogListener.IsDebugEnabled = false;
        fileLogListener.IsInfoEnabled = true;
        fileLogListener.IsWarningEnabled = true;
        fileLogListener.IsErrorEnabled = true;

        LogManager.AddListener(fileLogListener);
    }
}