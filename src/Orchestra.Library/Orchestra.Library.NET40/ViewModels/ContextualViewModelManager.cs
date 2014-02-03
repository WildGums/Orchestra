// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextualViewModelManager.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
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
        /// The _nested dock view model types
        /// </summary>
        private readonly Collection<Type> _nestedDockViewModelTypes = new Collection<Type>();

        /// <summary>
        /// The log
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

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

        #region Constructor
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
        /// Registers the nested dock view.
        /// </summary>
        public void RegisterNestedDockView<TNestedDockViewModel>()
        {
            _nestedDockViewModelTypes.Add(typeof(TNestedDockViewModel));
        }

        /// <summary>
        /// Determines whether type of the ViewModel belongs to a nested dock view.
        /// </summary>
        /// <param name="viewModel">The IViewModel.</param>
        /// <returns>
        ///   <c>true</c> if the type of the ViewModel belongs to a nested dock view; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNestedDockview(IViewModel viewModel)
        {
            return _nestedDockViewModelTypes.Contains(viewModel.GetType());
        }

        /// <summary>
        /// Registers 'contextual' view type, with the type of views that are context sensitive to this view.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <typeparam name="TContextSensitiveViewModel">The type of the context sensitive view model.</typeparam>
        /// <param name="title">The title.</param>
        /// <param name="dockLocation">The dock location.</param>
        [Obsolete("Title has to be set from the viewmodel. Use the RegisterContextualView without the title parameter.", true)]
        public void RegisterContextualView<TViewModel, TContextSensitiveViewModel>(string title, DockLocation dockLocation)
        {
            RegisterContextualView<TViewModel, TContextSensitiveViewModel>(new DockingSettings{DockLocation = dockLocation});
        }

        /// <summary>
        /// Registers 'contextual' view type, with the type of views that are context sensitive to this view.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <typeparam name="TContextSensitiveViewModel">The type of the context sensitive view model.</typeparam>
        /// <param name="title">The title.</param>
        /// <param name="dockingSettings">The docking settings.</param>
        [Obsolete("Title has to be set from the viewmodel. Use the RegisterContextualView without the title parameter.", true)]
        public void RegisterContextualView<TViewModel, TContextSensitiveViewModel>(string title, DockingSettings dockingSettings)
        {
            RegisterContextualView<TViewModel, TContextSensitiveViewModel>(dockingSettings);
        }

        /// <summary>
        /// Registers 'contextual' view type, with the type of views that are context sensitive to this view.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <typeparam name="TContextSensitiveViewModel">The type of the context sensitive view model.</typeparam>        
        /// <param name="dockLocation">The dock location.</param>
        public void RegisterContextualView<TViewModel, TContextSensitiveViewModel>(DockLocation dockLocation)
        {
            RegisterContextualView<TViewModel, TContextSensitiveViewModel>(new DockingSettings { DockLocation = dockLocation });
        }

        /// <summary>
        /// Registers 'contextual' view type, with the type of views that are context sensitive to this view.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <typeparam name="TContextSensitiveViewModel">The type of the context sensitive view model.</typeparam>        
        /// <param name="dockingSettings">The docking settings.</param>
        public void RegisterContextualView<TViewModel, TContextSensitiveViewModel>(DockingSettings dockingSettings)
        {
            Type viewModelType = typeof(TViewModel);
            Type contextSensitiveViewModelType = typeof(TContextSensitiveViewModel);

            if (!_contextualViewModelCollection.ContainsKey(viewModelType))
            {
                _contextualViewModelCollection.Add(viewModelType, new ContextSensitviveViewModelData(dockingSettings));
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

        /// <summary>
        /// Adds the context ensitive views to nested dock view.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="nestedDockingManager">The nested docking manager.</param>
        public void AddContextSensitiveViewsToNestedDockView(IViewModel viewModel, NestedDockingManager nestedDockingManager)
        {
            // viewModel is the viewmodel related to the nestedDockingManager
            var viewModelType = viewModel.GetType();

            // Open all, context sensitive vies related to the documentView
            if (_contextualViewModelCollection.ContainsKey(viewModelType))
            {
                foreach (var type in (_contextualViewModelCollection[viewModelType]).ContextDependentViewModels)
                {
                    IViewModel contextDependenViewModel;

                    if (_openContextSensitiveViews.All(v => v.GetType() != type))
                    {
                        try
                        {
                            contextDependenViewModel = _viewModelFactory.CreateViewModel(type, null);
                        }
                        catch (Exception ex)
                        {
                            Log.ErrorWithData(ex, "Error creating contextualViewModel.");
                            continue;
                        }                       

                        if (contextDependenViewModel != null)
                        {
                            DockingSettings dockingSettings = _contextualViewModelCollection[viewModelType].DockingSettings;
                            _orchestraService.ShowDocumentInNestedDockView(contextDependenViewModel, nestedDockingManager, dockingSettings);

                            if(_openContextSensitiveViews.All(v => v.GetType() != contextDependenViewModel.GetType()))
                            {                            
                                _openContextSensitiveViews.Add(contextDependenViewModel);                            
                            }

                            Log.Debug("ShowDocumentInNestedDockView: {0}, docklocation: {1}", contextDependenViewModel, dockingSettings.DockLocation);
                        }
                    }
                    else
                    {
                        contextDependenViewModel = _openContextSensitiveViews.FirstOrDefault(v => v.GetType() == type);

                        if (contextDependenViewModel != null)
                        {
                            DockingSettings dockingSettings = _contextualViewModelCollection[viewModelType].DockingSettings;
                            _orchestraService.ShowDocumentInNestedDockView(contextDependenViewModel, nestedDockingManager, dockingSettings);
                        }
                    }
                }
            }
        }

        private void ShowContextSensitiveViews(IDocumentView documentView)
        {
            Argument.IsNotNull(() => documentView);

            var vm = documentView.GetViewModel();
            var documentViewViewModelType = vm.GetType();

            if (IsNestedDockview(vm))
            {
                return;
            }

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
                            viewModel = _viewModelFactory.CreateViewModel(type, null);
                        }
                        catch (Exception ex)
                        {
                            Log.ErrorWithData(ex, "Error creating contextual ViewModel.");
                            continue;
                        }                        

                        if (viewModel != null && !_openContextSensitiveViews.Contains(viewModel))
                        {
                            var dockSettings = _contextualViewModelCollection[documentViewViewModelType].DockingSettings;
                            _orchestraService.ShowContextSensitiveDocument(viewModel, null, dockSettings);
                            _openContextSensitiveViews.Add(viewModel);

                            Log.Debug("Show context sensitive view: {0}, docklocation: {1}", viewModel, dockSettings.DockLocation);
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

            var vm = documentView.GetViewModel();
            return _contextualViewModelCollection.ContainsKey(vm.GetType());
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

            return (TViewModel)_openContextSensitiveViews.FirstOrDefault(vm => vm.GetType() == typeof(TViewModel));
        }

        /// <summary>
        /// Updates the contextual views.
        /// </summary>
        /// <param name="activatedView">The activated view.</param>
        public void UpdateContextualViews(DocumentView activatedView)
        {
            Argument.IsNotNull("The activated view", activatedView);

            var vm = activatedView.GetViewModel();
            if (vm == null || IsContextDependentViewModel(vm))
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

                var documentVm = document.GetViewModel();
                if (!IsContextDependentViewModel(documentVm) || HasContextualRelationShip(documentVm, vm))
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