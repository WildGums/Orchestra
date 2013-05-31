// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonItemBase.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Models
{
    using System;
    using System.Windows;
    using Catel;

    /// <summary>
    /// Defines a ribbon item base class
    /// </summary>
    public abstract class RibbonItemBase : IRibbonItem
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="RibbonItemBase" /> class from being created.
        /// </summary>
        /// <param name="tabItemHeader">The tab item header.</param>
        /// <param name="groupBoxHeader">The group box header.</param>
        /// <param name="itemHeader">The item header.</param>
        /// <param name="behavior">The behavior.</param>
        /// <exception cref="ArgumentException">The <paramref name="tabItemHeader"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentException">The <paramref name="groupBoxHeader"/> is <c>null</c> or whitespace.</exception>
        protected RibbonItemBase(string tabItemHeader, string groupBoxHeader, string itemHeader, RibbonBehavior behavior = RibbonBehavior.ActivateTab)
        {
            Argument.IsNotNullOrWhitespace("tabItemHeader", tabItemHeader);
            Argument.IsNotNullOrWhitespace("groupBoxHeader", groupBoxHeader);

            TabItemHeader = tabItemHeader;
            GroupBoxHeader = groupBoxHeader;
            ItemHeader = itemHeader;
            Behavior = behavior;

            OnlyShowWhenTabIsActivated = false;
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
        /// Gets or sets the ribbon item layout.
        /// </summary>
        /// <value>
        /// The ribbon item layout.
        /// </value>
        public IRibbonItemLayout Layout { get; set; }

        /// <summary>
        /// Gets or sets the ribbon item style.
        /// </summary>
        /// <value>
        /// The ribbon item style.
        /// </value>
        public Style Style { get; set; }

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