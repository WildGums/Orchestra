// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.Windows.Input;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using Orchestra;
using Orchestra.Services;
using InputGesture = Catel.Windows.Input.InputGesture;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static partial class ModuleInitializer
{
    static partial void InitializeSpecific()
    {
        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterType<IBaseColorService, BaseColorService>();
        serviceLocator.RegisterType<IFlyoutService, FlyoutService>();
        serviceLocator.RegisterType<IAboutService, MahAppsAboutService>();
        serviceLocator.RegisterType<IMessageService, MahAppsMessageService>();
        serviceLocator.RegisterType<IUIVisualizerService, MahAppsUIVisualizerService>();

        var commandManager = serviceLocator.ResolveType<ICommandManager>();

        commandManager.CreateCommand("Help.About", throwExceptionWhenCommandIsAlreadyCreated: false);
        commandManager.CreateCommand("Close", new InputGesture(Key.Escape), throwExceptionWhenCommandIsAlreadyCreated: false);

        var thirdPartyNoticesService = serviceLocator.ResolveType<IThirdPartyNoticesService>();
        thirdPartyNoticesService.AddWithTryCatch(() => new ResourceBasedThirdPartyNotice("ControlzEx", "https://github.com/ControlzEx/ControlzEx", "Orchestra.Shell.MahApps", "Orchestra.Orchestra.Shell.MahApps", "Resources.ThirdPartyNotices.controlzex.txt"));
        thirdPartyNoticesService.AddWithTryCatch(() => new ResourceBasedThirdPartyNotice("MahApps", "https://mahapps.com/", "Orchestra.Shell.MahApps", "Orchestra.Orchestra.Shell.MahApps", "Resources.ThirdPartyNotices.mahapps.txt"));
    }
}
