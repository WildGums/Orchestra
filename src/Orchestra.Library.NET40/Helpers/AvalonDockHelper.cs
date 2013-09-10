// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AvalonDockHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using Catel;
    using Catel.IoC;
    using Catel.Windows.Controls;
    using Microsoft.Practices.Prism.Regions;
    using Models;
    using Orchestra.Controls;
    using Orchestra.Views;
    using ViewModels;
    using Xceed.Wpf.AvalonDock;
    using Xceed.Wpf.AvalonDock.Layout;

    /// <summary>
    /// Helper class for avalon dock.
    /// </summary>
    public class AvalonDockHelper
    {
        #region Constants
        /// <summary>
        /// Docking manager reference.
        /// </summary>
        private static readonly DockingManager DockingManager;

        /// <summary>
        /// The layout document pane.
        /// </summary>
        private static readonly LayoutDocumentPane LayoutDocumentPane;

        /// <summary>
        /// The layout anchor group
        /// </summary>
        private static readonly LayoutAnchorGroup LayoutAnchorGroup;

        /// <summary>
        /// The layout anchorable pane on the right side.
        /// </summary>
        private static readonly LayoutAnchorablePane RightLayoutAnchorablePane;

        /// <summary>
        /// The layout anchorable pane on the left side.
        /// </summary>
        private static readonly LayoutAnchorablePane LeftLayoutAnchorablePane;

        // TODO MAVE: Only using the viewmodels -> should we keep the view? -> change view to viewmodel saves getting the viewmodel off of it. 
        // TODO MAVE: Keep this here, or create a ContextualService/ContextualHelper, rethink
        /// <summary>
        /// The documents collection, we need this collection to find relationships between views when the ActivatedView changes.
        /// </summary>
        private static readonly Collection<DocumentView> DocumentsCollection  = new Collection<DocumentView>();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes static members of the <see cref="AvalonDockHelper"/> class. 
        /// </summary>
        static AvalonDockHelper()
        {            
            DockingManager = ServiceLocator.Default.ResolveType<DockingManager>();
            DockingManager.DocumentClosed += OnDockingManagerDocumentClosed;

            DockingManager.ActiveContentChanged += DockingManagerActiveContentChanged;

            LayoutDocumentPane = ServiceLocator.Default.ResolveType<LayoutDocumentPane>();
            LayoutAnchorGroup = ServiceLocator.Default.ResolveType<LayoutAnchorGroup>();

            // TODO MAVE: Find the typed extension method
            RightLayoutAnchorablePane = (LayoutAnchorablePane)ServiceLocator.Default.ResolveType(typeof(LayoutAnchorablePane), "rightPropertiesPane");
            LeftLayoutAnchorablePane = (LayoutAnchorablePane)ServiceLocator.Default.ResolveType(typeof(LayoutAnchorablePane), "leftPropertiesPane"); 
        }        
        #endregion

        #region Properties
        /// <summary>
        /// Gets the region manager.
        /// </summary>
        private static IRegionManager RegionManager
        {
            get { return ServiceLocator.Default.ResolveType<IRegionManager>(); }
        }

        /// <summary>
        /// Gets or sets the activated view.
        /// </summary>
        /// <value>
        /// The activated view.
        /// </value>
        private static DocumentView ActivatedView { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <param name="viewType">Type of the view.</param>
        /// <param name="tag">The tag.</param>
        /// <returns>The found document or <c>null</c> if no document was found.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="viewType" /> is <c>null</c>.</exception>
        public static LayoutAnchorable FindDocument(Type viewType, object tag = null)
        {
            Argument.IsNotNull("viewType", viewType);

            return (from document in LayoutDocumentPane.Children where document is LayoutAnchorable && document.Content.GetType() == viewType && TagHelper.AreTagsEqual(tag, ((IView)document.Content).Tag) select document).Cast<LayoutAnchorable>().FirstOrDefault();
        }

        /// <summary>
        /// Activates the document in the docking manager, which makes it the active document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="document" /> is <c>null</c>.</exception>
        public static void ActivateDocument(LayoutAnchorable document)
        {
            Argument.IsNotNull("document", document);

            LayoutDocumentPane.SelectedContentIndex = LayoutDocumentPane.IndexOfChild(document);
        }

        /// <summary>
        /// Creates the document.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="dockLocation">The dock location.</param>
        /// <param name="contextualViewModel">The contextual parent view model.</param>
        /// <returns>
        /// The created layout document.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="view" /> is <c>null</c>.</exception>
        public static LayoutAnchorable CreateDocument(FrameworkElement view, object tag = null, DockLocation? dockLocation = null, ViewModelBase contextualViewModel = null)
        {
            // TODO MAVE: rethink name contextualViewModel..
            Argument.IsNotNull("view", view);

            bool isContextualView = false;
            var layoutDocument = WrapViewInLayoutDocument(view, tag, true );
            var documentView = view as DocumentView;

            if (documentView != null)
            {
                DocumentsCollection.Add(documentView);

                if (contextualViewModel != null)
                {
                    isContextualView = true;

                    var viewModel = documentView.ViewModel as ViewModelBase;

                    if (viewModel != null && !viewModel.ContextualViewModels.Contains(contextualViewModel))
                    {
                        viewModel.ContextualViewModels.Add(contextualViewModel);
                    }
                }
            }

           if (dockLocation != null)
            {                              
                switch (dockLocation)
	            {                      
	                case DockLocation.Bottom:
                        // TODO MAVE: Keep or not ...
                        throw new NotImplementedException();
                    case DockLocation.Left:
                        LeftLayoutAnchorablePane.Children.Add(layoutDocument);
	                    break;
                    case DockLocation.Right:
                        RightLayoutAnchorablePane.Children.Add(layoutDocument);
	                    break;                    
                    case DockLocation.Top:
                        // TODO MAVE: Keep or not ...
                        throw new NotImplementedException();
	            }                                                         
            }
            else
            {
                LayoutDocumentPane.Children.Add(layoutDocument);
            }

            // A new 'contextual' view has been added, now this must be set to visible or collapsed depending on the activated view.
           if (isContextualView && ActivatedView != null)
           {
               SetVisibilityForContextualViews();
           }

            return layoutDocument;
        }

        /// <summary>
        /// Wraps the view in a layout document.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="canFloat">if set to <c>true</c> [can float].</param>
        /// <returns>
        /// A wrapped layout document.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="view" /> is <c>null</c>.</exception>
        private static LayoutAnchorable WrapViewInLayoutDocument(FrameworkElement view, object tag = null, bool canFloat = false)
        {
            return new BindableLayoutDocument(view, tag, canFloat);
        }

        /// <summary>
        /// Called when the docking manager has just closed a document.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DocumentClosedEventArgs" /> instance containing the event data.</param>
        private static void OnDockingManagerDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            var containerView = e.Document;
            var view = containerView.Content as IDocumentView;
            if (view != null)
            {
                view.CloseDocument();
            }

            // var region = RegionManager.Regions[(string)view.Tag];
            // region.Remove(sender);
        }

        /// <summary>
        /// Closes the document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="document" /> is <c>null</c>.</exception>
        public static void CloseDocument(LayoutAnchorable document)
        {
            Argument.IsNotNull(() => document);

            document.Close();
        }

        /// <summary>
        /// Handles the ActiveContentChanged event of the DockingManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        static void DockingManagerActiveContentChanged(object sender, EventArgs e)
        {
            ActivatedView = ((DockingManager)sender).ActiveContent as DocumentView;                     
            SetVisibilityForContextualViews();
        }

        /// <summary>
        /// Sets the visibility for contextual views.
        /// </summary>        
        private static void SetVisibilityForContextualViews()
        {
            if (ActivatedView == null || ActivatedView.ViewModel == null)
            {
                return;
            }

            // Check what contextual documents have a relationship with the activated document, and set the visibility accordingly
            foreach (var document in DocumentsCollection)
            {
                if (ActivatedView.Equals(document))
                {
                    continue;
                }

                // TODO MAVE: This now only works for Orchestra.ViewModels.ViewModelBase .. is this ok?? rethink
                if (document.ViewModel is ViewModelBase)
                {
                    var viewModel = (ViewModelBase)document.ViewModel;

                    if (!viewModel.IsContextualViewModel || viewModel.ContextualViewModels.Contains((ViewModelBase)ActivatedView.ViewModel))
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
        #endregion
    }
}