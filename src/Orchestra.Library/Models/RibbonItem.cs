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
        /// Prevents a default instance of the <see cref="RibbonItem" /> class from being created.
        /// </summary>
        /// <param name="tabItemHeader">The tab item header.</param>
        /// <param name="groupBoxHeader">The group box header.</param>
        /// <param name="itemHeader">The item header.</param>
        /// <param name="behavior">The behavior.</param>
        /// <exception cref="ArgumentException">The <paramref name="tabItemHeader"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentException">The <paramref name="groupBoxHeader"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentException">The <paramref name="itemHeader"/> is <c>null</c> or whitespace.</exception>
        private RibbonItem(string tabItemHeader, string groupBoxHeader, string itemHeader, RibbonBehavior behavior = RibbonBehavior.ActivateTab)
        {
            Argument.IsNotNullOrWhitespace("tabItemHeader", tabItemHeader);
            Argument.IsNotNullOrWhitespace("groupBoxHeader", groupBoxHeader);
            Argument.IsNotNullOrWhitespace("itemHeader", itemHeader);

            TabItemHeader = tabItemHeader;
            GroupBoxHeader = groupBoxHeader;
            ItemHeader = itemHeader;
            Behavior = behavior;

            OnlyShowWhenTabIsActivated = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonItem"/> class.
        /// </summary>
        /// <param name="tabItemHeader">The tab item header.</param>
        /// <param name="groupBoxHeader">The group box header.</param>
        /// <param name="itemHeader">The item header.</param>
        /// <param name="command">The command.</param>
        /// <param name="behavior">The behavior.</param>
        /// <exception cref="ArgumentException">The <paramref name="tabItemHeader"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentException">The <paramref name="groupBoxHeader"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentException">The <paramref name="itemHeader"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="command"/> is <c>null</c>.</exception>
        public RibbonItem(string tabItemHeader, string groupBoxHeader, string itemHeader, ICommand command, RibbonBehavior behavior = RibbonBehavior.ActivateTab)
            : this(tabItemHeader, groupBoxHeader, itemHeader, behavior)
        {
            Argument.IsNotNull("command", command);

            Command = command;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonItem"/> class.
        /// </summary>
        /// <param name="tabItemHeader">The tab item header.</param>
        /// <param name="groupBoxHeader">The group box header.</param>
        /// <param name="itemHeader">The item header.</param>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="behavior">The behavior.</param>
        /// <exception cref="ArgumentException">The <paramref name="tabItemHeader"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentException">The <paramref name="groupBoxHeader"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentException">The <paramref name="itemHeader"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentException">The <paramref name="commandName"/> is <c>null</c> or whitespace.</exception>
        public RibbonItem(string tabItemHeader, string groupBoxHeader, string itemHeader, string commandName, RibbonBehavior behavior = RibbonBehavior.ActivateTab)
            : this(tabItemHeader, groupBoxHeader, itemHeader, behavior)
        {
            Argument.IsNotNullOrWhitespace("commandName", commandName);

            CommandName = commandName;
        }

        /// <summary>
        /// Gets or sets the name of the contextual tab item group.
        /// <para />
        /// This value is only used when <see cref="Context"/> is set to <see cref="RibbonContext.View"/>.
        /// </summary>
        /// <value>The name of the contextual tab item group.</value>
        public string ContextualTabItemGroupName { get; set; }

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
        /// Gets the name of the command.
        /// <para />
        /// The <see cref="Command" /> property always is used first. If that value is <c>null</c>, this value will be used
        /// to bind the command to the active document.
        /// </summary>
        /// <value>The name of the command.</value>
        public string CommandName { get; private set; }

        /// <summary>
        /// Gets the command.
        /// <para />
        /// If this command is set, it will be used directly. Otherwise a binding to the active document will be created
        /// using the <see cref="CommandName" /> property.
        /// </summary>
        /// <value>The command.</value>
        public ICommand Command { get; private set; }

        /// <summary>
        /// Gets a value indiciating whether this ribbon item should only be shown when 
        /// the tab is actually activated.
        /// </summary>
        public bool OnlyShowWhenTabIsActivated { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        public RibbonContext Context { get { return string.IsNullOrWhiteSpace(ContextualTabItemGroupName) ? RibbonContext.Global : RibbonContext.View; } }

        /// <summary>
        /// Gets or sets the behavior.
        /// </summary>
        /// <value>The behavior.</value>
        public RibbonBehavior Behavior { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}", TabItemHeader, GroupBoxHeader, ItemHeader);
        }
    }
}