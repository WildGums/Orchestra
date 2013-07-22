// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRibbonButton.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Models
{
    using System.Windows.Input;

    /// <summary>
    /// Provides a ribbon button methods and properties.
    /// </summary>
    public interface IRibbonButton : IRibbonControl
    {
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
    }
}