// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRibbonItem.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Models
{
    using System.Windows.Input;

    /// <summary>
    /// Class defining a ribbon item.
    /// </summary>
    public interface IRibbonItem
    {
        /// <summary>
        /// Gets the tab item header.
        /// </summary>
        /// <value>The tab item header.</value>
        string TabItemHeader { get; }

        /// <summary>
        /// Gets the group box header.
        /// </summary>
        /// <value>The group box header.</value>
        string GroupBoxHeader { get; }

        /// <summary>
        /// Gets the item header.
        /// </summary>
        /// <value>The item header.</value>
        string ItemHeader { get; }

        /// <summary>
        /// Gets or sets the item image.
        /// </summary>
        string ItemImage { get; set; }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>The command.</value>
        ICommand Command { get; }

        /// <summary>
        /// Gets a value indiciating whether this ribbon item should only be shown when 
        /// the tab is actually activated.
        /// </summary>
        bool OnlyShowWhenTabIsActivated { get; set; }
    }
}