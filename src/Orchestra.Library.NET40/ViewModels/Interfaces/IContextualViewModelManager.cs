// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContextualViewModelManager.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using Catel.MVVM;
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
        bool IsContextualViewModel(IViewModel viewModel);

        /// <summary>
        /// Determines whether this view model has a contextual relation ship with the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="contetxtualViewModel">The contetxtual view model.</param>
        /// <returns>
        ///   <c>true</c> if a contextual relation ship exists with the specified view model]; otherwise, <c>false</c>.
        /// </returns>
        bool HasContextualRelationShip(IViewModel viewModel, IViewModel contetxtualViewModel);

        /// <summary>
        /// Registers the <see cref="DocumentView"/>.
        /// Now that it is known in the IContextualViewModelManager, the visibility can be made contextsensitive.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        void RegisterDocumentView(DocumentView documentView);

        /// <summary>
        /// Registers the context view model.
        /// </summary>
        /// <param name="contextViewModel">The context view model the main view.</param>
        /// <param name="relatedViewModel">The related view model (properties for example).</param>
        void RegisterContextViewModel(IViewModel contextViewModel, IViewModel relatedViewModel);

        /// <summary>
        /// Sets the visibility for contextual views.
        /// </summary>
        /// <param name="activatedView">The activated view.</param>
        void SetVisibilityForContextualViews(DocumentView activatedView);
        #endregion
    }
}