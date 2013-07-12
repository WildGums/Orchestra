// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AvalonDockHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orchestra
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;

    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Windows.Controls;

    using Microsoft.Practices.Prism.Regions;
    using Orchestra.Controls;
    using Orchestra.Views;

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
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes static members of the <see cref="AvalonDockHelper"/> class. 
        /// </summary>
        static AvalonDockHelper()
        {
            DockingManager = ServiceLocator.Default.ResolveType<DockingManager>();
            DockingManager.DocumentClosed += OnDockingManagerDocumentClosed;

            LayoutDocumentPane = ServiceLocator.Default.ResolveType<LayoutDocumentPane>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the region manager.
        /// </summary>
        private static IRegionManager RegionManager
        {
            get
            {
                return ServiceLocator.Default.ResolveType<IRegionManager>();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <param name="viewType">Type of the view.</param>
        /// <param name="tag">The tag.</param>
        /// <returns>The found document or <c>null</c> if no document was found.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="viewType" /> is <c>null</c>.</exception>
        public static LayoutDocument FindDocument(Type viewType, object tag = null)
        {
            Argument.IsNotNull("viewType", viewType);

            return (from document in LayoutDocumentPane.Children where document is LayoutDocument && document.Content.GetType() == viewType && TagHelper.AreTagsEqual(tag, ((IView)document.Content).Tag) select document).Cast<LayoutDocument>().FirstOrDefault();
        }

        /// <summary>
        /// Activates the document in the docking manager, which makes it the active document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="document" /> is <c>null</c>.</exception>
        public static void ActivateDocument(LayoutDocument document)
        {
            Argument.IsNotNull("document", document);

            LayoutDocumentPane.SelectedContentIndex = LayoutDocumentPane.IndexOfChild(document);
        }

        /// <summary>
        /// Creates the document.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="tag">The tag.</param>
        /// <returns>The created layout document.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="view" /> is <c>null</c>.</exception>
        public static LayoutDocument CreateDocument(FrameworkElement view, object tag = null)
        {
            Argument.IsNotNull("view", view);

            var layoutDocument = WrapViewInLayoutDocument(view, tag);

            LayoutDocumentPane.Children.Add(layoutDocument);

            return layoutDocument;
        }

        /// <summary>
        /// Wraps the view in a layout document.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="tag">The tag.</param>
        /// <returns>A wrapped layout document.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="view" /> is <c>null</c>.</exception>
        private static LayoutDocument WrapViewInLayoutDocument(FrameworkElement view, object tag = null)
        {
            return new BindableLayoutDocument(view);
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
        #endregion
    }
}