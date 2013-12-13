// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineSeries.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Models
{
    using System;
    using System.Collections.Generic;
    using Catel;

    /// <summary>
    /// The line series.
    /// </summary>
    public class LineSeries
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentException">The <paramref name="name"/> is <c>null</c> or whitespace.</exception>
        public LineSeries(string name)
        {
            Argument.IsNotNull("name", name);

            Name = name;
            Points = new List<DataPoint>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <value>The points.</value>
        public List<DataPoint> Points { get; private set; }
        #endregion
    }
}