namespace Orchestra.Models
{
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// Provides a ribbon items collection methods and properties.
    /// </summary>
    public interface IRibbonItemsCollection : IRibbonItem
    {
        /// <summary>
        /// Gets or sets the items collection.
        /// </summary>
        /// <value>
        /// The items collection.
        /// </value>
        List<IRibbonItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the item container style.
        /// </summary>
        /// <value>
        /// The item container style.
        /// </value>
        Style ItemContainerStyle { get; set; }
    }
}