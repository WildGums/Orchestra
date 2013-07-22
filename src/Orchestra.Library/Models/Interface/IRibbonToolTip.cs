// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRibbonToolTip.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    /// <summary>
    /// Provides a ribbon tooltip methods and properties.
    /// </summary>
    public interface IRibbonToolTip
    {
        #region Properties
        /// <summary>
        /// Gets or sets the ribbon tooltip title.
        /// </summary>
        /// <value>
        /// The ribbon tooltip title.
        /// </value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the ribbon tooltip text.
        /// </summary>
        /// <value>
        /// The ribbon tooltip text.
        /// </value>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the ribbon tooltip width.
        /// </summary>
        /// <value>
        /// The ribbon tooltip width.
        /// </value>
        double Width { get; set; }
        #endregion
    }
}