// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonGallery.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a ribbon gallery.
    /// </summary>
    public class RibbonGallery : IRibbonGallery
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonGallery"/> class.
        /// </summary>
        public RibbonGallery()
        {
            ItemWidth = double.NaN;
            ItemHeight = double.NaN;
            MaxItemsInRow = 16;
            MinItemsInRow = 2;
        }
        #endregion

        #region IRibbonGallery Members
        /// <summary>
        /// Gets or sets the items collection.
        /// </summary>
        /// <value>
        /// The items collection.
        /// </value>
        public List<IRibbonItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the item container style.
        /// </summary>
        /// <value>
        /// The item container style.
        /// </value>
        public Style ItemContainerStyle { get; set; }

        /// <summary>
        /// Gets or sets the orientation of gallery.
        /// </summary>
        /// <value>
        /// The orientation of gallery.
        /// </value>
        public Orientation Orientation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IRibbonItem" /> is selectable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if selectable; otherwise, <c>false</c>.
        /// </value>
        public bool Selectable { get; set; }

        /// <summary>
        /// Gets or sets the width of the item.
        /// </summary>
        /// <value>
        /// The width of the item.
        /// </value>
        public double ItemWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the item.
        /// </summary>
        /// <value>
        /// The height of the item.
        /// </value>
        public double ItemHeight { get; set; }

        /// <summary>
        /// Gets or sets the min items in row.
        /// </summary>
        /// <value>
        /// The min items in row.
        /// </value>
        public int MinItemsInRow { get; set; }

        /// <summary>
        /// Gets or sets the max items in row.
        /// </summary>
        /// <value>
        /// The max items in row.
        /// </value>
        public int MaxItemsInRow { get; set; }
        #endregion
    }
}