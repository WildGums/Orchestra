// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationInitializationService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Threading.Tasks;
    using Catel.MVVM;

    public interface IApplicationInitializationService
    {
        /// <summary>
        /// Initialization code before creating the shell.
        /// </summary>
        /// <returns>Task.</returns>
        Task InitializeBeforeCreatingShell();

        /// <summary>
        /// Initialization code after creating the shell.
        /// </summary>
        /// <returns>Task.</returns>
        Task InitializeAfterCreatingShell();

        /// <summary>
        /// Initialization code before showing the shell.
        /// </summary>
        /// <returns>Task.</returns>
        Task InitializeBeforeShowingShell();

        /// <summary>
        /// Initialization code after showing the shell.
        /// </summary>
        /// <returns>Task.</returns>
        Task InitializeAfterShowingShell();
    }
}