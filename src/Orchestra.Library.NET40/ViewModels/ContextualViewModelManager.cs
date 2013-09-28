// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextualViewModelManager.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using Catel;
    using Catel.MVVM;
    using Models;
    using Orchestra.Views;
    using System;
    using Services;

    /// <summary>
    /// The ContextualViewModelManager manages views and there context sensitive views.
    /// </summary>
    public class ContextualViewModelManager : IContextualViewModelManager
    {
        #region Fields
        /// <summary>
        /// The _contextual view models
        /// A view (context) has one or more views related to this context 
        /// </summary>
        private readonly Dictionary<Type, ContextSensitviveViewModelData> _contextualViewModelCollection = new Dictionary<Type, ContextSensitviveViewModelData>();
        
        /// <summary>
        /// The collection of context sensitive views that are opened.
        /// </summary>
        private readonly Collection<IViewModel> _openContextSensitiveViews = new Collection<IViewModel>(); 

        /// <summary>
        /// The collection of documents that are opened in Orchestra, we need this collection to find relationships between views when the ActivatedView changes.
        /// </summary>
        private readonly Collection<IDocumentView> _openDocumentViewsCollection = new Collection<IDocumentView>();

        /// <summary>
        /// The <see cref="IOrchestraService">orchestra service</see>.
        /// </summary>
        private readonly IOrchestraService _orchestraService;

        /// <summary>
        /// The <see cref="IViewModelFactory">ViewModel factory</see>.
        /// </summary>
        private readonly IViewModelFactory _viewModelFactory;
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextualViewModelManager" /> class.
        /// </summary>
        /// <param name="orchestraService">The <see cref="IOrchestraService">orchestra service</see>.</param>
        /// <param name="viewModelFactory">The <see cref="IViewModelFactory">orchestra service</see>..</param>
        public ContextualViewModelManager(IOrchestraService orchestraService, IViewModelFactory viewModelFactory)
        {
            _orchestraService = orchestraService;
            _viewModelFactory = viewModelFactory;
        }
        #endregion

        #region IContextualViewModelManager Members
        /// <summary>
        /// Registers 'contextual' view type, with the type of views that are context sensitive to this view.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <typeparam name="TContextSensitiveViewModel">The type of the context sensitive view model.</typeparam>
        /// <param name="title">The title.</param>
        /// <param name="dockLocation">The dock location.</param>
        public void RegisterContextualView<TViewModel, TContextSensitiveViewModel>(string title, DockLocation dockLocation)
        {
            Type viewModelType = typeof(TViewModel);
            Type contextSensitiveViewModelType = typeof(TContextSensitiveViewModel);

            if (!_contextualViewModelCollection.ContainsKey(viewModelType))
            {
                _contextualViewModelCollection.Add(viewModelType, new ContextSensitviveViewModelData(title, dockLocation));
            }            

            if (!_contextualViewModelCollection[viewModelType].ContextDependentViewModels.Contains(contextSensitiveViewModelType))
            {
                _contextualViewModelCollection[viewModelType].ContextDependentViewModels.Add(contextSensitiveViewModelType);
            }            
        }

        /// <summary>
        /// Registers the <see cref="DocumentView" />.
        /// Now that it is known in the IContextualViewModelManager, the visibility of the context sensitive views can be managed.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        public void RegisterOpenDocumentView(IDocumentView documentView)
        {
            Argument.IsNotNull(() => documentView);

            if (!_openDocumentViewsCollection.Contains(documentView))
            {
                _openDocumentViewsCollection.Add(documentView);
                ShowContextSensitiveViews(documentView);
            }
        }
        
        /// <summary>
        /// Unregisters the contextual document view.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        public void UnregisterDocumentView(IDocumentView documentView)
        {
            Argument.IsNotNull(() => documentView);

            _openDocumentViewsCollection.Remove(documentView);                        
        }
        
        private void ShowContextSensitiveViews(IDocumentView documentView)
        {
            Argument.IsNotNull(() => documentView);

            var documentViewViewModelType = documentView.ViewModel.GetType();

            // Does this viewtype have a contextsensitive view associated with it?
            if (HasContextSensitiveViewAssociated(documentView))
            {
                // Yes, are it's contextsensitive's views already opened?
                if (_openContextSensitiveViews.All(viewModel => viewModel.GetType() != documentViewViewModelType))
                {                    
                    // Open all, context sensitive vies related to the documentView
                    foreach (var type in (_contextualViewModelCollection[documentViewViewModelType]).ContextDependentViewModels)
                    {
                        IViewModel viewModel;
                        
                        if (_openContextSensitiveViews.Any(v => v.GetType() == type))
                        {
                            continue;
                        }

                        try
                        {
                            viewModel = (ViewModelBase)Activator.CreateInstance(type, _contextualViewModelCollection[documentViewViewModelType].Title);
                            //viewModel = (IViewModel)_viewModelFactory.CreateViewModel(type, null);
                            //viewModel.Title = _contextualViewModelCollection[documentView.ViewModel.GetType()].Title;
                        }
                        catch (Exception)
                        {                            
                            continue;
                        }
                        
                        if (!_openContextSensitiveViews.Contains(viewModel))
                        {
                            _orchestraService.ShowDocument(viewModel, null, _contextualViewModelCollection[documentViewViewModelType].DockLocation);
                            _openContextSensitiveViews.Add(viewModel);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether this view has context sensitive views associated.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        /// <returns>
        ///   <c>true</c> if the view has context sensitive view(s) associated, otherwise, <c>false</c>.
        /// </returns>
        private bool HasContextSensitiveViewAssociated(IDocumentView documentView)
        {
            Argument.IsNotNull(() => documentView);

            return _contextualViewModelCollection.ContainsKey(documentView.ViewModel.GetType());
        }

        /// <summary>
        /// Determines whether [is context dependent view model] [the specified view model].
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>
        ///   <c>true</c> if [is context dependent view model] [the specified view model]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsContextDependentViewModel(IViewModel viewModel)
        {
            Argument.IsNotNull(() => viewModel);

            return _contextualViewModelCollection.Any(item => item.Value.ContextDependentViewModels.Contains(viewModel.GetType()));
        }

        /// <summary>
        /// Determines whether the viewModel has a contextual relation ship with the contextual view model.
        /// </summary>
        /// <param name="contextSensitiveviewModel">The context sensitiveview model.</param>
        /// <param name="contextualViewModel">The contetxtual view model.</param>
        /// <returns>
        ///   <c>true</c> if [has contextual relation ship] [the specified view model]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasContextualRelationShip(IViewModel contextSensitiveviewModel, IViewModel contextualViewModel)
        {
            Argument.IsNotNull(() => contextSensitiveviewModel);
            Argument.IsNotNull(() => contextualViewModel);

            if (_contextualViewModelCollection.Count == 0)
            {
                return false;
            }

            if (_contextualViewModelCollection.ContainsKey(contextualViewModel.GetType()))
            {
                var contextualViewModelCollection = _contextualViewModelCollection[contextualViewModel.GetType()];

                if (contextualViewModelCollection != null && contextualViewModelCollection.ContextDependentViewModels.Count > 0)
                {
                    return _contextualViewModelCollection[contextualViewModel.GetType()].ContextDependentViewModels.Contains(contextSensitiveviewModel.GetType());
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the view model for context sensitive view.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <returns></returns>
        public TViewModel GetViewModelForContextSensitiveView<TViewModel>()
        {
            if (_openContextSensitiveViews.Count == 0)
            {
                // Retun null
                return default(TViewModel);
            }

            return (TViewModel) _openContextSensitiveViews.FirstOrDefault(vm => vm.GetType() == typeof(TViewModel));            
        }

        /// <summary>
        /// Updates the contextual views.
        /// </summary>
        /// <param name="activatedView">The activated view.</param>
        public void UpdateContextualViews(DocumentView activatedView)
        {            
            Argument.IsNotNull("The activated view", activatedView);

            if (activatedView.ViewModel == null || IsContextDependentViewModel(activatedView.ViewModel))
            {
                return;
            }

            // Check what contextual documents have a relationship with the activated document, and set the visibility accordingly
            foreach (var document in _openDocumentViewsCollection)
            {
                if (activatedView.Equals(document))
                {
                    continue;
                }

                if (!IsContextDependentViewModel(document.ViewModel) || HasContextualRelationShip(document.ViewModel, activatedView.ViewModel))
                {
                    ((DocumentView)document).Visibility = Visibility.Visible;
                }
                else
                {
                    ((DocumentView)document).Visibility = Visibility.Collapsed;
                }
            }
        }
        #endregion
    }   
}