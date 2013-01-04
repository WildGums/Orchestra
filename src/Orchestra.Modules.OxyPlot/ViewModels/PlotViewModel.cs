// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.OxyPlot.ViewModels
{
    using System.Collections.ObjectModel;
    using Catel;
    using Orchestra.ViewModels;
    using global::OxyPlot;

    /// <summary>
    /// Plot view model.
    /// </summary>
    public class PlotViewModel : ViewModelBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PlotViewModel"/> class.
        /// </summary>
        public PlotViewModel(ObservableCollection<int> x, ObservableCollection<int> y)
        {
            Argument.IsNotNull("x", x);
            Argument.IsNotNull("y", y);

            X = x;
            Y = y;

            PlotModel = CreateLinePlotModel();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return "Plot"; }
        }

        /// <summary>
        /// X values.
        /// </summary>
        public ObservableCollection<int> X { get; private set; }

        /// <summary>
        /// Y values.
        /// </summary>
        public ObservableCollection<int> Y { get; private set; }

        /// <summary>
        /// Gets the OxyPlot model.
        /// </summary>
        public PlotModel PlotModel { get; private set; }
        #endregion

        #region Commands
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets
        #endregion

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets

        private PlotModel CreateLinePlotModel()
        {
            // Create the plot model
            var model = new PlotModel("Dynamic plot");

            // Create two line series (markers are hidden by default)
            var series = new LineSeries("") { MarkerType = MarkerType.Circle };
            for (int i = 0; i < X.Count; i++)
            {
                series.Points.Add(new DataPoint(X[i], Y[i]));
            }

            // Add the series to the plot model
            model.Series.Add(series);

            return model;
        }
    }
}