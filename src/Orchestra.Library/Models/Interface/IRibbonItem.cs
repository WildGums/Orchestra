// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRibbonItem.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Models
{
    using System.Windows;

    /// <summary>
    /// The ribbon context.
    /// </summary>
    public enum RibbonContext
    {
        /// <summary>
        /// Global, which means always visible.
        /// </summary>
        Global,

        /// <summary>
        /// View, which means only when the view it is registered with is visible.
        /// </summary>
        View
    }

    /// <summary>
    /// The ribbon behaviors.
    /// </summary>
    public enum RibbonBehavior
    {
        /// <summary>
        /// No specific behavior.
        /// </summary>
        None,

        /// <summary>
        /// Activates the tab.
        /// </summary>
        ActivateTab
    }

    /// <summary>
    /// Provides a common ribbon item methods and properties.
    /// </summary>
    public interface IRibbonItem
    {
        /// <summary>
        /// Gets or sets the name of the contextual tab item group.
        /// <para />
        /// This value is only used when <see cref="Context"/> is set to <see cref="RibbonContext.View"/>.
        /// </summary>
        /// <value>The name of the contextual tab item group.</value>
        string ContextualTabItemGroupName { get; set; }

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
        /// Gets a value indiciating whether this ribbon item should only be shown when 
        /// the tab is actually activated.
        /// </summary>
        bool OnlyShowWhenTabIsActivated { get; set; }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        RibbonContext Context { get; }

        /// <summary>
        /// Gets or sets the behavior.
        /// </summary>
        /// <value>The behavior.</value>
        RibbonBehavior Behavior { get; set; }

        /// <summary>
        /// Gets the ribbon item layout.
        /// </summary>
        /// <value>
        /// The ribbon item layout.
        /// </value>
        IRibbonItemLayout Layout { get; }

        /// <summary>
        /// Gets the ribbon item style.
        /// </summary>
        /// <value>
        /// The ribbon item style.
        /// </value>
        Style Style { get; }
    }
}