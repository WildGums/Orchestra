// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrchestraService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Services
{
    using System;
    using Catel.MVVM;

    /// <summary>
    /// The orchestra service that allows communication with the shell.
    /// </summary>
    public interface IOrchestraService
    {
        /// <summary>
        /// Shows the document in the main shell.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        void ShowDocument<TViewModel>()
            where TViewModel : IViewModel, new();

        /// <summary>
        /// Shows the document in the main shell.
        /// </summary>
        /// <param name="viewModel">The view model to show which will automatically be resolved to a view.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="viewModel"/> is <c>null</c>.</exception>
        void ShowDocument<TViewModel>(TViewModel viewModel)
            where TViewModel : IViewModel;
    }
}