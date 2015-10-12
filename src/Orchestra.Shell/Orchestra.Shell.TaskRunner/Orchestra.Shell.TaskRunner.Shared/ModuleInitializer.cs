// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.Windows.Input;
using Catel.IoC;
using Catel.MVVM;
using InputGesture = Catel.Windows.Input.InputGesture;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static partial class ModuleInitializer
{
    static partial void InitializeSpecific()
    {
        var serviceLocator = ServiceLocator.Default;

        var commandManager = serviceLocator.ResolveType<ICommandManager>();

        commandManager.CreateCommand("Help.About", throwExceptionWhenCommandIsAlreadyCreated: false);
        commandManager.CreateCommand("Groups.Run", new InputGesture(Key.R, ModifierKeys.Shift | ModifierKeys.Control));

        commandManager.CreateCommand("Runner.Run", new InputGesture(Key.F5), throwExceptionWhenCommandIsAlreadyCreated: false);
        commandManager.CreateCommand("Runner.ClearConsole", throwExceptionWhenCommandIsAlreadyCreated: false);
    }
}