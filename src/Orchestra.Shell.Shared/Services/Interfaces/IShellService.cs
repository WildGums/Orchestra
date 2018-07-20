// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShellService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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
        [ObsoleteEx(Message = "App is now constructed with splash by default. Splash can be hidden by using ShowSplash = false", TreatAsErrorFromVersion = "5.0", RemoveInVersion = "6.0")]
        Task<TShell> CreateWithSplashAsync<TShell>()
            where TShell : class, IShell;

        /// <summary>
        /// Creates a new shell.
        /// </summary>
        /// <typeparam name="TShell">The type of the shell.</typeparam>
        /// <returns>The created shell.</returns>
        /// <exception cref="OrchestraException">The shell is already created and cannot be created again.</exception>
        Task<TShell> CreateAsync<TShell>()
            where TShell : class, IShell;
    }
}
