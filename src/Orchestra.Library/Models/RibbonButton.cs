// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonButton.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Models
{
    using System;
    using System.Windows.Input;
    using Catel;

    /// <summary>
    /// Represents a ribbon button
    /// </summary>
    public class RibbonButton : RibbonControlBase, IRibbonButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonButton"/> class.
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
        public RibbonButton(string tabItemHeader, string groupBoxHeader, string itemHeader, ICommand command, RibbonBehavior behavior = RibbonBehavior.ActivateTab)
            : base(tabItemHeader, groupBoxHeader, itemHeader, behavior)
        {
            Argument.IsNotNullOrWhitespace("itemHeader", itemHeader);
            Argument.IsNotNull("command", command);

            Command = command;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonButton"/> class.
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
        public RibbonButton(string tabItemHeader, string groupBoxHeader, string itemHeader, string commandName, RibbonBehavior behavior = RibbonBehavior.ActivateTab)
            : base(tabItemHeader, groupBoxHeader, itemHeader, behavior)
        {
            Argument.IsNotNullOrWhitespace("itemHeader", itemHeader);
            Argument.IsNotNullOrWhitespace("commandName", commandName);

            CommandName = commandName;
        }

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
    }
}