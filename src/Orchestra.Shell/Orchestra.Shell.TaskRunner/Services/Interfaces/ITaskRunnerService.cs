// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITaskRunnerService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Windows;

    public interface ITaskRunnerService
    {
        /// <summary>
        /// Gets the data context that will be set to the view.
        /// </summary>
        /// <returns>The data context or <c>null</c>.</returns>
        object GetViewDataContext();

        /// <summary>
        /// Gets the view to show in the configuration.
        /// </summary>
        /// <returns>FrameworkElement.</returns>
        FrameworkElement GetView();

        /// <summary>
        /// Runs the logic when the run button is clicked.
        /// </summary>
        /// <param name="dataContext">The data context retrieved earlier using the <see cref="GetViewDataContext"/> method.</param>
        void Run(object dataContext);
    }
}