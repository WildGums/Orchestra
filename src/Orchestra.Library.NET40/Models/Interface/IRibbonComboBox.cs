// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRibbonComboBox.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Models
{
    /// <summary>
    /// Provides a ribbon combo box methods and properties.
    /// </summary>
    public interface IRibbonComboBox : IRibbonControl
    {
        /// <summary>
        /// Gets the items source.
        /// </summary>
        /// <value>
        /// The items source.
        /// </value>
        string ItemsSource { get; }
        /// <summary>
        /// Gets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        string SelectedItem { get; }
    }
}