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
        /// <para />
        /// If this command is set, it will be used directly. Otherwise a binding to the active document will be created 
        /// using the <see cref="CommandName"/> property.
        /// </summary>
        /// <value>The command.</value>
        ICommand Command { get; }

        /// <summary>
        /// Gets the name of the command.
        /// <para />
        /// The <see cref="Command"/> property always is used first. If that value is <c>null</c>, this value will be used
        /// to bind the command to the active document.
        /// </summary>
        /// <value>The name of the command.</value>
        string CommandName { get; }

        /// <summary>
        /// Gets a value indiciating whether this ribbon item should only be shown when 
        /// the tab is actually activated.
        /// </summary>
        bool OnlyShowWhenTabIsActivated { get; set; }
    }
}