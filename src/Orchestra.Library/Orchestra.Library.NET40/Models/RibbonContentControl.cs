// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonContentControl.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using System.Windows;

    /// <summary>
    /// Interface for RibbonItem with the ability to show dynamic content
    /// </summary>
    public interface IRibbonContentControl
    {
        #region Properties
        /// <summary>
        /// Gets or sets ContentTemplate for the custom content.
        /// </summary>
        /// <value>The item image.</value>
        DataTemplate ContentTemplate { get; set; }
        #endregion
    }

    /// <summary>
    /// RibbonItem with the ability to show dynamic content.
    /// </summary>
    public class RibbonContentControl : RibbonControlBase, IRibbonContentControl
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonContentControl"/> class.
        /// </summary>
        /// <param name="tabItemHeader">The tab item header.</param>
        /// <param name="groupBoxHeader">The group box header.</param>
        /// <param name="itemHeader">The item header.</param>
        /// <param name="behavior">The behavior.</param>
        public RibbonContentControl(string tabItemHeader, string groupBoxHeader, string itemHeader = null, RibbonBehavior behavior = RibbonBehavior.ActivateTab)
            : base(tabItemHeader, groupBoxHeader, itemHeader, behavior)
        {
        }
        #endregion

        #region IRibbonContentControl Members
        /// <summary>
        /// Gets or sets ContentTemplate for the custom content.
        /// </summary>
        /// <value>The item image.</value>
        public DataTemplate ContentTemplate { get; set; }
        #endregion
    }
}