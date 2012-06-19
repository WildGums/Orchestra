// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonItem.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Models
{
    using System;
    using System.Windows.Input;
    using Catel;

    /// <summary>
    /// Implementation of the <see cref="IRibbonItem" />.
    /// </summary>
    public class RibbonItem : IRibbonItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonItem"/> class.
        /// </summary>
        /// <param name="tabItemHeader">The tab item header.</param>
        /// <param name="groupBoxHeader">The group box header.</param>
        /// <param name="itemHeader">The item header.</param>
        /// <param name="command">The command.</param>
        /// <exception cref="ArgumentException">The <paramref name="tabItemHeader"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentException">The <paramref name="groupBoxHeader"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentException">The <paramref name="itemHeader"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="command"/> is <c>null</c>.</exception>
        public RibbonItem(string tabItemHeader, string groupBoxHeader, string itemHeader, ICommand command)
        {
            Argument.IsNotNullOrWhitespace("tabItemHeader", tabItemHeader);
            Argument.IsNotNullOrWhitespace("groupBoxHeader", groupBoxHeader);
            Argument.IsNotNullOrWhitespace("itemHeader", itemHeader);
            Argument.IsNotNull("command", command);

            TabItemHeader = tabItemHeader;
            GroupBoxHeader = groupBoxHeader;
            ItemHeader = itemHeader;
            Command = command;
        }

        /// <summary>
        /// Gets the tab item header.
        /// </summary>
        /// <value>The tab item header.</value>
        public string TabItemHeader { get; private set; }

        /// <summary>
        /// Gets the group box header.
        /// </summary>
        /// <value>The group box header.</value>
        public string GroupBoxHeader { get; private set; }

        /// <summary>
        /// Gets the item header.
        /// </summary>
        /// <value>The item header.</value>
        public string ItemHeader { get; private set; }

        /// <summary>
        /// Gets or sets the item image.
        /// </summary>
        /// <value>The item image.</value>
        public string ItemImage { get; set; }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>The command.</value>
        public ICommand Command { get; private set; }
    }
}