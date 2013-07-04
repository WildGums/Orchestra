// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStatusBarService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orchestra.Services
{
    using System;

    /// <summary>
    /// Class to modify the status bar.
    /// </summary>
    public interface IStatusBarService
    {
        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="status"/> is <c>null</c>.</exception>
        void UpdateStatus(string status);
    }
}