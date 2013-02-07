// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModelHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.OxyPlot
{
    using System;
    using Catel;
    using global::OxyPlot;
    using global::OxyPlot.Series;

    /// <summary>
    /// Plot model helper.
    /// </summary>
    public static class PlotModelHelper
    {
        #region Methods
        /// <summary>
        /// Transports a plot model into an actual plot model.
        /// </summary>
        /// <param name="plotModel">The plot model to transform.</param>
        /// <returns>The converted plot model.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="plotModel"/> is <c>null</c>.</exception>
        public static PlotModel TransformPlotModelIntoActualPlotModel(this global::OxyPlot.Models.OxyPlotModel plotModel)
        {
            Argument.IsNotNull("plotModel", plotModel);

            var model = new PlotModel(plotModel.Name);
            model.LegendTitle = plotModel.Legend;

            switch (plotModel.SerieType)
            {
                case global::OxyPlot.Models.SerieTypes.Line:
                    CopyLineModelData(plotModel, model);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("SerieType");
            }

            return model;
        }

        private static void CopyLineModelData(global::OxyPlot.Models.OxyPlotModel sourcePlotModel, PlotModel targetPlotModel)
        {
            Argument.IsNotNull("sourcePlotModel", sourcePlotModel);
            Argument.IsNotNull("targetPlotModel", targetPlotModel);

            foreach (var series in sourcePlotModel.Series)
            {
                var targetSeries = new LineSeries(series.Name);
                foreach (var point in series.Points)
                {
                    targetSeries.Points.Add(new DataPoint(point.X, point.Y));
                }

                targetPlotModel.Series.Add(targetSeries);
            }
        }
        #endregion
    }
}