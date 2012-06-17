// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrchestraService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Services
{
    using System;
    using Catel;
    using Catel.MVVM;

    /// <summary>
    /// Orchestra service implementation.
    /// </summary>
    public class OrchestraService : ServiceBase, IOrchestraService
    {
        /// <summary>
        /// Shows the document in the main shell.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        public void ShowDocument<TViewModel>() 
            where TViewModel : IViewModel, new()
        {
            var viewModel = new TViewModel();

            ShowDocument(viewModel);
        }

        /// <summary>
        /// Shows the document in the main shell.
        /// </summary>
        /// <param name="viewModel">The view model to show which will automatically be resolved to a view.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="viewModel"/> is <c>null</c>.</exception>
        public void ShowDocument<TViewModel>(TViewModel viewModel) 
            where TViewModel : IViewModel
        {
            Argument.IsNotNull("viewModel", viewModel);

            var viewLocator = GetService<IViewLocator>();
            var viewType = viewLocator.ResolveView(viewModel.GetType());

            var view = ViewHelper.ConstructViewWithViewModel(viewType, viewModel);

            AvalonDockHelper.ActivateDocumentContent(view, "MainRegion");
        }
    }
}