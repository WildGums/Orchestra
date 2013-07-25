// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonItem.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Models
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Implementation of the <see cref="IRibbonButton" />.
    /// </summary>
    [ObsoleteEx(Replacement = "RibbonButton", RemoveInVersion = "2.0", TreatAsErrorFromVersion = "1.0")]
    public class RibbonItem : RibbonButton
    {
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
            : base(tabItemHeader, groupBoxHeader, itemHeader, command, behavior)
        {
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
            : base(tabItemHeader, groupBoxHeader, itemHeader, commandName, behavior)
        {
        }
    }
}