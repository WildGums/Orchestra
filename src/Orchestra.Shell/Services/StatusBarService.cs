// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusBarService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orchestra.Services
{
    using System;

    using Catel;

    using Orchestra.Views;

    /// <summary>
    /// Servie to control the status bar.
    /// </summary>
    public class StatusBarService : IStatusBarService
    {
        private readonly MainWindow _shell;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusBarService" /> class.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="shell"/> is <c>null</c>.</exception>
        public StatusBarService(MainWindow shell)
        {
            Argument.IsNotNull(() => shell);

            _shell = shell;
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="status"/> is <c>null</c>.</exception>
        public void UpdateStatus(string status)
        {
            Argument.IsNotNull(() => status);

            _shell.StatusBar = status;
        }
    }
}