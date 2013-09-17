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
        /// Determines whether this view model has a contextual relation ship with the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="contextualViewModel">The contetxtual view model.</param>
        /// <returns>
        ///   <c>true</c> if a contextual relation ship exists with the specified view model]; otherwise, <c>false</c>.
        /// </returns>
        bool HasContextualRelationShip(IViewModel viewModel, IViewModel contextualViewModel);

        /// <summary>
        /// Registers the <see cref="DocumentView"/>.
        /// Now that it is known in the IContextualViewModelManager, the visibility can be made contextsensitive.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        void RegisterDocumentView(IDocumentView documentView);
       
        /// <summary>
        /// Sets the visibility for contextual views.
        /// </summary>
        /// <param name="activatedView">The activated view.</param>
        void SetVisibilityForContextualViews(IDocumentView activatedView);
        #endregion

        /// <summary>
        /// Registers the context sensitive parent view.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TP"></typeparam>
        void RegisterContextualView<T, TP>(string title, DockLocation dockLocation);

        /// <summary>
        /// Unregisters the contextual document view.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        void UnregisterDocumentView(IDocumentView documentView);
    }
}