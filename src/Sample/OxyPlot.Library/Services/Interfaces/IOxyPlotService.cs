// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOxyPlotService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Services
{
    using Models;

    /// <summary>
    /// Interface exposing all external OxyPlot interaction possibilities.
    /// </summary>
    public interface IOxyPlotService
    {
        /// <summary>
        /// Plots the specified plotModel model.
        /// </summary>
        /// <param name="plotModel">The plotModel.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="plotModel"/> is <c>null</c>.</exception>
        void Plot(OxyPlotModel plotModel);
    }
}