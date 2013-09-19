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
    using System.Windows.Threading;
    using Catel;
    using Catel.MVVM;
    using Catel.Windows.Controls;
    using Models;
    using Orchestra.Views;
    using System;
    using Services;
    using Xceed.Wpf.AvalonDock.Layout;

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

        // TODO: remove openview when closed (btw how to reopen.. needs to be registered and opened from menu)
        /// <summary>
        /// The collection of context sensitive views that are opened.
        /// </summary>
        private readonly Collection<IViewModel> _openContextSensitiveViews = new Collection<IViewModel>(); 

        /// <summary>
        /// The collection of documents that are opened in Orchestra, we need this collection to find relationships between views when the ActivatedView changes.
        /// </summary>
        private readonly Collection<IDocumentView> _openDocumentViewsCollection = new Collection<IDocumentView>();

        /// <summary>
        /// Collection of context dependent views, that are currently hidden. 
        /// </summary>
        private static readonly Collection<LayoutAnchorable> HiddenViews = new Collection<LayoutAnchorable>();

        #endregion

        #region IContextualViewModelManager Members
        /// <summary>
        /// Registers 'contextual' view type, with the type of views that are context sensitive to this view.
        /// </summary>
        /// <typeparam name="T">The type for the 'contextual' view, this is the view, other views are context sensitive with.</typeparam>
        /// <typeparam name="TP">The type for the context sensitive view.</typeparam>
        public void RegisterContextualView<T, TP>(string title, DockLocation dockLocation)
        {
            Type type = typeof(T);
            Type contextSensitiveType = typeof(TP);

            if (!_contextualViewModelCollection.ContainsKey(type))
            {
                _contextualViewModelCollection.Add(type, new ContextSensitviveViewModelData(title, dockLocation));
            }            

            if (!_contextualViewModelCollection[type].ContextDependentViewModels.Contains(contextSensitiveType))
            {
                _contextualViewModelCollection[type].ContextDependentViewModels.Add(contextSensitiveType);
            }
        }

        /// <summary>
        /// Registers the <see cref="DocumentView" />.
        /// Now that it is known in the IContextualViewModelManager, the visibility of the context sensitive views can be managed.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        public void RegisterDocumentView(IDocumentView documentView)
        {
            if (!_openDocumentViewsCollection.Contains(documentView))
            {
                _openDocumentViewsCollection.Add(documentView);
                ShowContextSensitiveViews(documentView);
            }
        }

        // TODO: unregister when document is closed, and cleanup everything.
        /// <summary>
        /// Unregisters the contextual document view.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        public void UnregisterDocumentView(IDocumentView documentView)
        {
            _openDocumentViewsCollection.Remove(documentView);            
            HideContextSensitiveViews(documentView);
        }

        /// <summary>
        /// Are there any open documents in orchestra, with this Type.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <returns><c>True</c> when there are open documents with this Type, otherwise <c>false</c></returns>
        private bool TypeHasOpenDocuments(Type documentType)
        {
            return _openDocumentViewsCollection.Any(document => document.GetType() == documentType);
        }

        private void HideContextSensitiveViews(IDocumentView documentView)
        {                       
            if (HasContextSensitiveViewAssociated(documentView))
            {
                SetVisibilityForContextualViews(documentView);
            } 
        }

        private void ShowContextSensitiveViews(IDocumentView documentView)
        {
            // Does this viewtype have a contextsensitive view associated with it?
            if (HasContextSensitiveViewAssociated(documentView))
            {
                // Yes, are it's contextsensitive's views already opened?
                if (_openContextSensitiveViews.All(viewModel => viewModel.GetType() != documentView.ViewModel.GetType()))
                {
                    // No..
                    var os = (IOrchestraService) Catel.IoC.ServiceLocator.Default.GetService(typeof (IOrchestraService));

                    // Open all, context sensitive vies related to the documentView
                    foreach (var type in (_contextualViewModelCollection[documentView.ViewModel.GetType()]).ContextDependentViewModels)
                    {
                        IViewModel viewModel;

                        try
                        {
                            viewModel = (IViewModel)Activator.CreateInstance(type, _contextualViewModelCollection[documentView.ViewModel.GetType()].Title);
                        }
                        catch (Exception)
                        {
                            // What if the viewmodel's constructor does not have a title property?
                            // Create a derrived ViewModel type for this?
                            continue;
                        }
                        
                        if (!_openContextSensitiveViews.Contains(viewModel))
                        {
                            os.ShowDocument(viewModel, null, _contextualViewModelCollection[documentView.ViewModel.GetType()].DockLocation);
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
            return _contextualViewModelCollection.Any(item => item.Value.ContextDependentViewModels.Contains(viewModel.GetType()));
        }

        /// <summary>
        /// Determines whether the viewModel has a contextual relation ship with the contextual view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="contextualViewModel">The contetxtual view model.</param>
        /// <returns>
        ///   <c>true</c> if [has contextual relation ship] [the specified view model]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasContextualRelationShip(IViewModel viewModel, IViewModel contextualViewModel)
        {
            if (_contextualViewModelCollection == null || _contextualViewModelCollection.Count == 0)
            {
                return false;
            }

            if (_contextualViewModelCollection.ContainsKey(contextualViewModel.GetType()))
            {
                var contextualViewModelCollection = _contextualViewModelCollection[contextualViewModel.GetType()];

                if (contextualViewModelCollection != null && contextualViewModelCollection.ContextDependentViewModels.Count > 0)
                {
                    return _contextualViewModelCollection[contextualViewModel.GetType()].ContextDependentViewModels.Contains(viewModel.GetType());
                }
            }
            return false;
        }

        /// <summary>
        /// Sets the visibility for contextual views.
        /// </summary>        
        public void SetVisibilityForContextualViews(IDocumentView activatedView)
        {
            Argument.IsNotNull("The activated view", activatedView);

            // when a context dependend view is opened, keep the current state.
            if (activatedView.ViewModel == null || IsContextDependentViewModel(activatedView.ViewModel))
            {
                return;
            }

            ((FrameworkElement)activatedView).Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            {
                // Check what contextual documents have a relationship with the activated document, and set the visibility accordingly
                foreach (var document in _openDocumentViewsCollection)
                {
                    if (activatedView.Equals(document))
                    {
                        continue;
                    }
                
                    // When the document is not context sensitive, leave it alone.
                    if (!IsContextDependentViewModel(document.ViewModel))
                    {
                        continue;
                    }

                    if (HasContextualRelationShip(document.ViewModel, activatedView.ViewModel))
                    {
                        ShowDocument(document, null);                        
                    }
                    else
                    {
                        HideDocument(document, null);                        
                    }
                }
            }));
        }

        /// <summary>
        /// Gets the view model for context sensitive view.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IViewModel GetViewModelForContextSensitiveView<T>()
        {
            if (_openContextSensitiveViews == null || _openContextSensitiveViews.Count == 0)
            {
                return null;
            }

            return _openContextSensitiveViews.FirstOrDefault(vm => vm.GetType() == typeof (T));            
        }

        /// <summary>
        /// Hides all context sensitive views.
        /// </summary>
        public void HideAllContextSensitiveViews()
        {
            foreach (var document in _openDocumentViewsCollection)
            {                
                // When the document is not context sensitive, leave it alone.
                if (!IsContextDependentViewModel(document.ViewModel))
                {
                    continue;
                }
                
                HideDocument(document, null);                                    
            }  
        }

        /// <summary>
        /// Hides the document.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        /// <param name="tag">The tag.</param>
        private void HideDocument(IDocumentView documentView, object tag)
        {
            var document = AvalonDockHelper.FindDocument(documentView.GetType(), tag);

            if (document != null)
            {
                HiddenViews.Add(document);

                if (document.Dispatcher.CheckAccess())
                {
                    document.Hide();
                }
                else
                {
                    document.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => document.Hide()));
                }
            }
        }

        /// <summary>
        /// Shows the document.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        /// <param name="tag">The tag.</param>
        private void ShowDocument(IDocumentView documentView, object tag)
        {
            var doc = (from document in HiddenViews where document != null && document.Content.GetType() == documentView.GetType() && TagHelper.AreTagsEqual(tag, ((IView)document.Content).Tag) select document).FirstOrDefault();

            if (doc != null)
            {
                doc.Show();
                HiddenViews.Remove(doc);
            }
        }
        #endregion
    }   
}