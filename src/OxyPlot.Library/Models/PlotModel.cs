// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Plot.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Models
{
    using System;
    using System.Collections.Generic;
    using Catel;

    /// <summary>
    /// Defines plot data that can be drawn in OxyPlot.
    /// </summary>
    public class PlotModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlotModel" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="legend">The legend.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="legend"/> is <c>null</c>.</exception>
        public PlotModel(string name, string legend)
        {
            Argument.IsNotNull("name", name);
            Argument.IsNotNull("legend", legend);

            Name = name;
            Legend = legend;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the legend.
        /// </summary>
        /// <value>The legend.</value>
        public string Legend { get; private set; }

        /// <summary>
        /// Gets or sets the X label.
        /// </summary>
        /// <value>The X label.</value>
        public string XLabel { get; set; }

        /// <summary>
        /// Gets or sets the X values.
        /// </summary>
        /// <value>The X values.</value>
        public List<double> XValues { get; set; }

        /// <summary>
        /// Gets or sets the Y label.
        /// </summary>
        /// <value>The Y label.</value>
        public string YLabel { get; set; }

        /// <summary>
        /// Gets or sets the Y values.
        /// </summary>
        /// <value>The Y values.</value>
        public List<double> YValues { get; set; }
    }
}