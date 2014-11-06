// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShellService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Views;

    public partial interface IShellService
    {
        /// <summary>
        /// Gets the shell.
        /// </summary>
        /// <value>The shell.</value>
        IShell Shell { get; }

        /// <summary>
        /// Creates a new shell and shows a splash during the initialization.
        /// </summary>
        /// <typeparam name="TShell">The type of the shell.</typeparam>
        /// <returns>The created shell.</returns>
        /// <exception cref="OrchestraException">The shell is already created and cannot be created again.</exception>
        Task<TShell> CreateWithSplash<TShell>()
            where TShell : IShell;

        /// <summary>
        /// Creates a new shell and shows a splash during the initialization.
        /// </summary>
        /// <typeparam name="TShell">The type of the shell.</typeparam>
        /// <param name="preInitialize">The pre initialize handler to initialize custom logic. If <c>null</c>, this value will be ignored.</param>
        /// <param name="initializeCommands">The initialize commands handler. If <c>null</c>, no commands will be initialized.</param>
        /// <param name="postInitialize">The post initialize handler to initialize custom logic. If <c>null</c>, this value will be ignored.</param>
        /// <returns>The created shell.</returns>
        /// <exception cref="OrchestraException">The shell is already created and cannot be created again.</exception>
        [ObsoleteEx(Replacement = "CreateWithSplash<TShell>() in combination with IApplicationInitializationService", TreatAsErrorFromVersion = "2.0", RemoveInVersion = "3.0")]
        Task<TShell> CreateWithSplash<TShell>(Func<Task> preInitialize, Func<ICommandManager, Task> initializeCommands = null, Func<Task> postInitialize = null)
            where TShell : IShell;

        /// <summary>
        /// Creates a new shell.
        /// </summary>
        /// <typeparam name="TShell">The type of the shell.</typeparam>
        /// <returns>The created shell.</returns>
        /// <exception cref="OrchestraException">The shell is already created and cannot be created again.</exception>
        Task<TShell> Create<TShell>()
            where TShell : IShell;

        /// <summary>
        /// Creates a new shell.
        /// </summary>
        /// <typeparam name="TShell">The type of the shell.</typeparam>
        /// <param name="preInitialize">The pre initialize handler to initialize custom logic. If <c>null</c>, this value will be ignored.</param>
        /// <param name="initializeCommands">The initialize commands handler. If <c>null</c>, no commands will be initialized.</param>
        /// <param name="postInitialize">The post initialize handler to initialize custom logic. If <c>null</c>, this value will be ignored.</param>
        /// <returns>The created shell.</returns>
        /// <exception cref="OrchestraException">The shell is already created and cannot be created again.</exception>
        [ObsoleteEx( Replacement = "Create<TShell>() in combination with IApplicationInitializationService", TreatAsErrorFromVersion = "2.0", RemoveInVersion = "3.0")]
        Task<TShell> Create<TShell>(Func<Task> preInitialize, Func<ICommandManager, Task> initializeCommands = null, Func<Task> postInitialize = null)
            where TShell : IShell;
    }
}