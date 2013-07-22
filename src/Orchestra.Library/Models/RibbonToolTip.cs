// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonToolTip.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    /// <summary>
    /// Represents a ribbon tooltip.
    /// </summary>
    public class RibbonToolTip : IRibbonToolTip
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonToolTip"/> class.
        /// </summary>
        public RibbonToolTip()
        {
            Width = double.NaN;
        }
        #endregion

        #region IRibbonToolTip Members
        /// <summary>
        /// Gets or sets the ribbon tooltip title.
        /// </summary>
        /// <value>
        /// The ribbon tooltip title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the ribbon tooltip text.
        /// </summary>
        /// <value>
        /// The ribbon tooltip text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the ribbon tooltip width.
        /// </summary>
        /// <value>
        /// The ribbon tooltip width.
        /// </value>
        public double Width { get; set; }
        #endregion
    }
}