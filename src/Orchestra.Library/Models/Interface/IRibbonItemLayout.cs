// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRibbonItemLayout.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Models
{
    /// <summary>
    /// Provides information about the ribbon item layout.
    /// </summary>
    public interface IRibbonItemLayout
    {
        // TODO: add row number
        // TODO: add size driven behaviour support

        /// <summary>
        /// Gets the width of the ribbon item.
        /// </summary>
        /// <value>
        /// The width of the ribbon item.
        /// </value>
        double Width { get; }
    }
}