// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyPlotService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.OxyPlot.Services
{
    using System;
    using Catel;
    using Orchestra.Services;
    using ViewModels;
    using global::OxyPlot.Models;
    using global::OxyPlot.Services;

    /// <summary>
    /// The main entrance for all external applications to OxyPlot.
    /// </summary>
    public class OxyPlotService : ServiceBase, IOxyPlotService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPlotService" /> class.
        /// </summary>
        public OxyPlotService()
        {
        }

        /// <summary>
        /// Plots the specified plotModel model.
        /// </summary>
        /// <param name="plotModel">The plotModel.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="plotModel"/> is <c>null</c>.</exception>
        public void Plot(PlotModel plotModel)
        {
            Argument.IsNotNull("plotModel", plotModel);

            var orchestraService = GetService<IOrchestraService>();

            var viewModel = new OxyPlotViewModel(plotModel);
            orchestraService.ShowDocument(viewModel);
        }
    }
}