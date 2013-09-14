namespace Orchestra
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using Catel;
    using Catel.MVVM;
    using Views;
    using ViewModelBase = ViewModels.ViewModelBase;

    /// <summary>
    /// The ContextualViewModelManager manages views and there contextual views
    /// </summary>
    public interface IContextualViewModelManager
    {
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
    }

    /// <summary>
    /// The ContextualViewModelManager manages views and there contextual views
    /// </summary>
    public class ContextualViewModelManager : IContextualViewModelManager
    {

        /// <summary>
        /// The documents collection, we need this collection to find relationships between views when the ActivatedView changes.
        /// </summary>
        private readonly Collection<DocumentView> _documentViewsCollection = new Collection<DocumentView>();

        /// <summary>
        /// The _contextual view models
        /// </summary>
        private readonly Dictionary<IViewModel, ICollection<IViewModel>> _contextualViewModels = new Dictionary<IViewModel, ICollection<IViewModel>>();

        /// <summary>
        /// Registers the <see cref="DocumentView" />.
        /// Now that it is known in the IContextualViewModelManager, the visibility can be made contextsensitive.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        public void RegisterDocumentView(DocumentView documentView)
        {
            _documentViewsCollection.Add(documentView);
        }

        /// <summary>
        /// Registers the context view model.
        /// </summary>
        /// <param name="contextViewModel">The context view model the main view.</param>
        /// <param name="relatedViewModel">The related view model (properties for example).</param>
        public void RegisterContextViewModel(IViewModel contextViewModel, IViewModel relatedViewModel)
        {
            if (_contextualViewModels.ContainsKey(contextViewModel))
            {
                // The view related to the context is not yet registered
                if (!_contextualViewModels[contextViewModel].Contains(relatedViewModel))
                {
                    _contextualViewModels[contextViewModel].Add(relatedViewModel);
                }

                return;
            }

            _contextualViewModels.Add(contextViewModel, new Collection<IViewModel>());
            _contextualViewModels[contextViewModel].Add(relatedViewModel);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is contextual view model.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is contextual view model; otherwise, <c>false</c>.
        /// </value>
        public bool IsContextualViewModel(IViewModel viewModel)
        {
            return _contextualViewModels.ContainsKey(viewModel);

            //return _contextualViewModels.Any(pair => pair.Value.Contains(viewModel));
        }

        /// <summary>
        /// Determines whether this view model has a contextual relation ship with the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="contetxtualViewModel">The contetxtual view model.</param>
        /// <returns>
        ///   <c>true</c> if a contextual relation ship exists with the specified view model]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasContextualRelationShip(IViewModel viewModel, IViewModel contetxtualViewModel)
        {

            if (_contextualViewModels == null || _contextualViewModels.Count == 0)
            {
                return false;
            }


            if (_contextualViewModels.ContainsKey(viewModel))
            {
                var contextualViewModelCollection = _contextualViewModels[viewModel];

                if (contextualViewModelCollection != null && contextualViewModelCollection.Count > 0)
                {
                    return _contextualViewModels[viewModel].Contains(contetxtualViewModel);
                }
            }
            return false;
        }

        /// <summary>
        /// Sets the visibility for contextual views.
        /// </summary>        
        public void SetVisibilityForContextualViews(DocumentView activatedView)
        {
            Argument.IsNotNull("The activated view", activatedView);

            if (activatedView.ViewModel == null)
            {
                return;
            }

            // Check what contextual documents have a relationship with the activated document, and set the visibility accordingly
            foreach (var document in _documentViewsCollection)
            {
                if (activatedView.Equals(document))
                {
                    continue;
                }

                if (!IsContextualViewModel(document.ViewModel) || HasContextualRelationShip(document.ViewModel, activatedView.ViewModel))
                {
                    document.Visibility = Visibility.Visible;
                }
                else
                {
                    document.Visibility = Visibility.Collapsed;
                }                
            }
        }
    }
}
