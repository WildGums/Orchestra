// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonSplitButton.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Represents a ribbon split button
    /// </summary>
    public class RibbonSplitButton : RibbonButton, IRibbonSplitButton
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonSplitButton"/> class.
        /// </summary>
        /// <param name="tabItemHeader">The tab item header.</param>
        /// <param name="groupBoxHeader">The group box header.</param>
        /// <param name="itemHeader">The item header.</param>
        /// <param name="command">The command.</param>
        /// <param name="behavior">The behavior.</param>
        public RibbonSplitButton(string tabItemHeader, string groupBoxHeader, string itemHeader, ICommand command, RibbonBehavior behavior = RibbonBehavior.ActivateTab)
            : base(tabItemHeader, groupBoxHeader, itemHeader, command, behavior)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonSplitButton"/> class.
        /// </summary>
        /// <param name="tabItemHeader">The tab item header.</param>
        /// <param name="groupBoxHeader">The group box header.</param>
        /// <param name="itemHeader">The item header.</param>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="behavior">The behavior.</param>
        public RibbonSplitButton(string tabItemHeader, string groupBoxHeader, string itemHeader, string commandName, RibbonBehavior behavior = RibbonBehavior.ActivateTab)
            : base(tabItemHeader, groupBoxHeader, itemHeader, commandName, behavior)
        {
        }
        #endregion

        #region IRibbonSplitButton Members
        /// <summary>
        /// Gets or sets the gallery items.
        /// </summary>
        /// <value>
        /// The gallery items.
        /// </value>
        public List<IRibbonItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the item container style.
        /// </summary>
        /// <value>
        /// The item container style.
        /// </value>
        public Style ItemContainerStyle { get; set; }
        #endregion
    }
}