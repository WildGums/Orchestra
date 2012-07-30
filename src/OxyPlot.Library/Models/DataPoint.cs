// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataPoint.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Models
{
    /// <summary>
    /// The datapoint class.
    /// </summary>
    public class DataPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataPoint" /> class.
        /// </summary>
        public DataPoint()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPoint" /> class.
        /// </summary>
        public DataPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>The X.</value>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>The Y.</value>
        public double Y { get; set; }
    }
}