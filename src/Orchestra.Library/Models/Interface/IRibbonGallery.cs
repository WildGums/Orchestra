namespace Orchestra.Models
{
    using System.Windows.Controls;

    /// <summary>
    /// Provides a ribbon gallery methods and properties.
    /// </summary>
    public interface IRibbonGallery : IRibbonItemsCollection
    {
        /// <summary>
        /// Gets or sets the orientation of gallery.
        /// </summary>
        /// <value>
        /// The orientation of gallery.
        /// </value>
        Orientation Orientation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IRibbonItem"/> is selectable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if selectable; otherwise, <c>false</c>.
        /// </value>
        bool Selectable { get; set; }

        /// <summary>
        /// Gets or sets the width of the item.
        /// </summary>
        /// <value>
        /// The width of the item.
        /// </value>
        double ItemWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the item.
        /// </summary>
        /// <value>
        /// The height of the item.
        /// </value>
        double ItemHeight { get; set; }

        /// <summary>
        /// Gets or sets the min items in row.
        /// </summary>
        /// <value>
        /// The min items in row.
        /// </value>
        int MinItemsInRow { get; set; }

        /// <summary>
        /// Gets or sets the max items in row.
        /// </summary>
        /// <value>
        /// The max items in row.
        /// </value>
        int MaxItemsInRow { get; set; }
    }
}