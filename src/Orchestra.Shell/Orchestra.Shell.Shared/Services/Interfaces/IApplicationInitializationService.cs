// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationInitializationService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Threading.Tasks;

    public interface IApplicationInitializationService
    {
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