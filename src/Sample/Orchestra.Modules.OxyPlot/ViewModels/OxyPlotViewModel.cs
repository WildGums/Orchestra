// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyPlotViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.OxyPlot.ViewModels
{
    using System;
    using Catel;
    using global::OxyPlot;
    using global::OxyPlot.Axes;
    using global::OxyPlot.Series;

    /// <summary>
    /// PlotDemo view model.
    /// </summary>
    public class OxyPlotViewModel : Orchestra.ViewModels.ViewModelBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPlotViewModel" /> class.
        /// </summary>
        /// <param name="plotModel">The plot model.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="plotModel"/> is <c>null</c>.</exception>
        public OxyPlotViewModel(global::OxyPlot.Models.OxyPlotModel plotModel)
        {
            Argument.IsNotNull("plotModel", plotModel);

            PlotModel = plotModel.TransformPlotModelIntoActualPlotModel();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return "OxyPlot"; }
        }

        /// <summary>
        /// Gets the OxyPlot model.
        /// </summary>
        public PlotModel PlotModel { get; private set; }
        #endregion

        #region Methods
        private PlotModel CreateSquareWaveDemoModel()
        {
            var model = new PlotModel("Square wave");
            var ls = new LineSeries("sin(x)+sin(3x)/3+sin(5x)/5+...");
            int n = 10;

            for (double x = -10; x < 10; x += 0.0001)
            {
                double y = 0;
                for (int i = 0; i < n; i++)
                {
                    int j = i * 2 + 1;
                    y += Math.Sin(j * x) / j;
                }

                ls.Points.Add(new DataPoint(x, y));
            }

            model.Series.Add(ls);
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -4, 4));
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom));

            return model;
        }

        private PlotModel CreateLinePlotModel()
        {
            // Create the plot model
            var model = new PlotModel("Simple example", "using OxyPlot");

            // Create two line series (markers are hidden by default)
            var series1 = new LineSeries("Series 1") { MarkerType = MarkerType.Circle };
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(10, 18));
            series1.Points.Add(new DataPoint(20, 12));
            series1.Points.Add(new DataPoint(30, 8));
            series1.Points.Add(new DataPoint(40, 15));

            var series2 = new LineSeries("Series 2") { MarkerType = MarkerType.Square };
            series2.Points.Add(new DataPoint(0, 4));
            series2.Points.Add(new DataPoint(10, 12));
            series2.Points.Add(new DataPoint(20, 16));
            series2.Points.Add(new DataPoint(30, 25));
            series2.Points.Add(new DataPoint(40, 5));

            // Add the series to the plot model
            model.Series.Add(series1);
            model.Series.Add(series2);

            return model;
        }

        private PlotModel CreatePieModel()
        {
            var model = new PlotModel();
            model.Title = "World population by continent";
            // http://www.nationsonline.org/oneworld/world_population.htm
            // http://en.wikipedia.org/wiki/Continent

            var ps = new PieSeries();
            ps.Slices.Add(new PieSlice("Africa", 1030) { IsExploded = true });
            ps.Slices.Add(new PieSlice("Americas", 929) { IsExploded = true });
            ps.Slices.Add(new PieSlice("Asia", 4157));
            ps.Slices.Add(new PieSlice("Europe", 739) { IsExploded = true });
            ps.Slices.Add(new PieSlice("Oceania", 35) { IsExploded = true });
            ps.InnerDiameter = 0.2;
            ps.ExplodedDistance = 0;
            ps.Stroke = OxyColors.Black;
            ps.StrokeThickness = 1.0;
            ps.AngleSpan = 360;
            ps.StartAngle = 0;
            model.Series.Add(ps);

            return model;
        }

        private PlotModel CreatePolarModel()
        {
            var model = new PlotModel("Polar plot", "Archimedean spiral with equation r(θ) = θ for 0 < θ < 6π")
            {
                PlotType = PlotType.Polar,
                PlotMargins = new OxyThickness(20, 20, 4, 40),
                PlotAreaBorderThickness = 0
            };

            var maxAngle = Math.PI * 2;
            var majorStep = Math.PI / 4;
            var minorStep = Math.PI / 16;
            
            model.Axes.Add(
                new AngleAxis(0, maxAngle, majorStep, minorStep)
                {
                    FormatAsFractions = true,
                    FractionUnit = Math.PI,
                    FractionUnitSymbol = "π"
                });
            model.Axes.Add(new MagnitudeAxis());
            model.Series.Add(new FunctionSeries(t => t, t => t, 0, Math.PI * 6, 0.01));

            return model;
        }
        #endregion
    }
}