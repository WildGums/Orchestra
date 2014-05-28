// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.Windows.Input;
using Catel.IoC;
using Catel.MVVM;
using FallDownMatrixManager.Services;
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

        serviceLocator.RegisterType<IFlyoutService, FlyoutService>();

        var commandManager = serviceLocator.ResolveType<ICommandManager>();
        commandManager.CreateCommand("Close", new InputGesture(Key.Escape), throwExceptionWhenCommandIsAlreadyCreated: false);
    }
}