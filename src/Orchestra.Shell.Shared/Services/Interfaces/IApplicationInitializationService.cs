// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationInitializationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Threading.Tasks;

    public interface IApplicationInitializationService
    {
        /// <summary>
        /// Gets a value indicating whether the splash screen should be shown.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the splash screen should be shown; otherwise, <c>false</c>.
        /// </value>
        bool ShowSplashScreen { get; }

        /// <summary>
        /// Gets a value indicating whether the shell should be shown.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the shell should be shown; otherwise, <c>false</c>.
        /// </value>
        bool ShowShell { get; }

        /// <summary>
        /// Initialization code before showing the splash screen.
        /// </summary>
        /// <returns>Task.</returns>
        Task InitializeBeforeShowingSplashScreenAsync();

        /// <summary>
        /// Initialization code before creating the shell.
        /// </summary>
        /// <returns>Task.</returns>
        Task InitializeBeforeCreatingShellAsync();

        /// <summary>
        /// Initialization code after creating the shell.
        /// </summary>
        /// <returns>Task.</returns>
        Task InitializeAfterCreatingShellAsync();

        /// <summary>
        /// Initialization code before showing the shell.
        /// </summary>
        /// <returns>Task.</returns>
        Task InitializeBeforeShowingShellAsync();

        /// <summary>
        /// Initialization code after showing the shell.
        /// </summary>
        /// <returns>Task.</returns>
        Task InitializeAfterShowingShellAsync();
    }
}
