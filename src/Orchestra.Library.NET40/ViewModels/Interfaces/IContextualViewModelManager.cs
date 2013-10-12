// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContextualViewModelManager.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using Catel.MVVM;
    using Models;
    using Orchestra.Views;

    /// <summary>
    /// The ContextualViewModelManager manages views and there contextual views
    /// </summary>
    public interface IContextualViewModelManager
    {
        #region Methods
        /// <summary>
        /// Gets a value indicating whether this instance is contextual view model.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is contextual view model; otherwise, <c>false</c>.
        /// </value>
        bool IsContextDependentViewModel(IViewModel viewModel);

        /// <summary>
        /// Determines whether type of the ViewModel belongs to a nested dock view.
        /// </summary>
        /// <param name="viewModel">The IViewModel.</param>
        /// <returns>
        ///   <c>true</c> if the type of the ViewModel belongs to a nested dock view; otherwise, <c>false</c>.
        /// </returns>
        bool IsNestedDockview(IViewModel viewModel);

        /// <summary>
        /// Determines whether this view model has a contextual relation ship with the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="contextualViewModel">The contetxtual view model.</param>
        /// <returns>
        ///   <c>true</c> if a contextual relation ship exists with the specified view model]; otherwise, <c>false</c>.
        /// </returns>
        bool HasContextualRelationShip(IViewModel viewModel, IViewModel contextualViewModel);

        /// <summary>
        /// Registers the nested dock view.
        /// </summary>
        void RegisterNestedDockView<TNestedDockViewModel>();

        /// <summary>
        /// Registers the <see cref="DocumentView"/>.
        /// Now that it is known in the IContextualViewModelManager, the visibility can be made contextsensitive.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        void RegisterOpenDocumentView(IDocumentView documentView);

        /// <summary>
        /// Registers the context sensitive parent view.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <typeparam name="TContextSensitiveViewModel">The type of the context sensitive view model.</typeparam>
        /// <param name="title">The title.</param>
        /// <param name="dockLocation">The dock location.</param>
        void RegisterContextualView<TViewModel, TContextSensitiveViewModel>(string title, DockLocation dockLocation);

        /// <summary>
        /// Unregisters the contextual document view.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        void UnregisterDocumentView(IDocumentView documentView);

        /// <summary>
        /// Adds the context sensitive views to nested dock view.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="nestedDockingManager">The nested docking manager.</param>
        void AddContextSensitiveViewsToNestedDockView(IViewModel viewModel, NestedDockingManager nestedDockingManager);

        /// <summary>
        /// Gets the view model for context sensitive view.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <returns></returns>
        TViewModel GetViewModelForContextSensitiveView<TViewModel>();

        /// <summary>
        /// Updates the contextual views.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        void UpdateContextualViews(DocumentView documentView);
        #endregion        
    }
}