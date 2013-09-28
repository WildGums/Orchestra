// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AvalonDockHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Windows.Controls;
    using Microsoft.Practices.Prism.Regions;
    using Models;    
    using Orchestra.Controls;
    using Orchestra.Views;
    using Xceed.Wpf.AvalonDock;
    using Xceed.Wpf.AvalonDock.Layout;
    using ViewModelBase = ViewModels.ViewModelBase;

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

        /// <summary>
        /// The <see cref="IContextualViewModelManager" />
        /// </summary>
        private static readonly IContextualViewModelManager ContextualViewModelManager;

        /// <summary>
        /// The layout anchor group on the bottom side.
        /// </summary>
        private static readonly LayoutAnchorGroup BottomPropertiesPane;

        /// <summary>
        /// The layout anchor group on the Top.
        /// </summary>
        private static readonly LayoutAnchorGroup TopPropertiesPane;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes static members of the <see cref="AvalonDockHelper"/> class. 
        /// </summary>
        static AvalonDockHelper()
        {
            ContextualViewModelManager = ServiceLocator.Default.ResolveType<IContextualViewModelManager>();
            DockingManager = ServiceLocator.Default.ResolveType<DockingManager>();
            DockingManager.DocumentClosed += OnDockingManagerDocumentClosed;

            DockingManager.ActiveContentChanged += DockingManagerActiveContentChanged;

            LayoutDocumentPane = ServiceLocator.Default.ResolveType<LayoutDocumentPane>();
            LayoutAnchorGroup = ServiceLocator.Default.ResolveType<LayoutAnchorGroup>();            
            
            RightLayoutAnchorablePane = (LayoutAnchorablePane)ServiceLocator.Default.ResolveType(typeof(LayoutAnchorablePane), "rightPropertiesPane");
            LeftLayoutAnchorablePane = (LayoutAnchorablePane)ServiceLocator.Default.ResolveType(typeof(LayoutAnchorablePane), "leftPropertiesPane");
            BottomPropertiesPane = (LayoutAnchorGroup)ServiceLocator.Default.ResolveType(typeof(LayoutAnchorGroup), "bottomPropertiesPane");
            TopPropertiesPane = (LayoutAnchorGroup)ServiceLocator.Default.ResolveType(typeof(LayoutAnchorGroup), "topPropertiesPane");             
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

            LayoutAnchorable doc = null;

            var documents = DockingManager.Layout.Descendents().OfType<LayoutAnchorable>();

            foreach (var layoutAnchorable in documents)
            {
                if (layoutAnchorable.Content.GetType() == viewType && TagHelper.AreTagsEqual(tag, ((IView)layoutAnchorable.Content).Tag))
                {
                    doc = layoutAnchorable;
                    break;
                }
            }
            
            return doc;
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
        /// Dock a view to the specified <see cref="DockLocation" />
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="dockLocation">The <see cref="DockLocation" />.</param>
        public static void DockView(LayoutAnchorable document, DockLocation dockLocation)
        {
            Debug.WriteLine("DockView");

            switch (dockLocation)
            {
                case DockLocation.Bottom:
                    BottomPropertiesPane.Children.Add(document);
                    break;
                case DockLocation.Left:
                    LeftLayoutAnchorablePane.Children.Add(document);
                    break;
                case DockLocation.Right:
                    RightLayoutAnchorablePane.Children.Add(document);
                    document.CanFloat = true;                    
                    break;
                case DockLocation.Top:
                    TopPropertiesPane.Children.Add(document);
                    break;
            }
        }

        /// <summary>
        /// Creates the document.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="dockLocation">The dock location.</param>
        /// <returns>
        /// The created layout document.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="view" /> is <c>null</c>.</exception>
        public static LayoutAnchorable CreateDocument(FrameworkElement view, object tag = null, DockLocation? dockLocation = null)
        {            
            Argument.IsNotNull("view", view);
            
            var layoutDocument = WrapViewInLayoutDocument(view, tag, true );
            var documentView = view as DocumentView;            

            ContextualViewModelManager.RegisterOpenDocumentView(documentView);

            if (dockLocation == null)
            {
                LayoutDocumentPane.Children.Add(layoutDocument);
            }
            else
            {
                DockView(layoutDocument, (DockLocation)dockLocation);
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
                ContextualViewModelManager.UnregisterDocumentView(view);
            }                        
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

            if (ActivatedView != null && ActivatedView.ViewModel is IContextualViewModel)
            {
                ((IContextualViewModel)ActivatedView.ViewModel).ViewModelActivated();
            }

            ContextualViewModelManager.UpdateContextualViews(ActivatedView);
        }        
        #endregion
    }
}