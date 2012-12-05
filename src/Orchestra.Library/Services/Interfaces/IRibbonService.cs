// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRibbonService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Services
{
    using System;
    using Models;

    /// <summary>
    /// Interface definition of the ribbon service.
    /// </summary>
    public interface IRibbonService
    {
        /// <summary>
        /// Adds the specified ribbon item to the main ribbon.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        void AddRibbonItem(IRibbonItem ribbonItem);

        /// <summary>
        /// Removes the specified ribbon item to the main ribbon.
        /// <para />
        /// This method will ignore calls when the item is not available in the ribbon.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        void RemoveRibbonItem(IRibbonItem ribbonItem);
    }
}