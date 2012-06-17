// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AvalonDockHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra
{
    using System;
    using System.Windows;
    using AvalonDock;
    using AvalonDock.Layout;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Microsoft.Practices.Prism.Regions;

    /// <summary>
    /// Helper class for avalon dock.
    /// </summary>
    public class AvalonDockHelper
    {
        /// <summary>
        /// Docking manager reference.
        /// </summary>
        private static readonly DockingManager DockingManager;

        /// <summary>
        /// The layout document pane.
        /// </summary>
        private static readonly LayoutDocumentPane LayoutDocumentPane;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        static AvalonDockHelper()
        {
            DockingManager = ServiceLocator.Instance.ResolveType<DockingManager>();
            DockingManager.DocumentClosed += OnDockingManagerDocumentClosed;

            LayoutDocumentPane = ServiceLocator.Instance.ResolveType<LayoutDocumentPane>();
        }

        #region Properties
        /// <summary>
        /// Gets the region manager.
        /// </summary>
        private static IRegionManager RegionManager
        {
            get { return ServiceLocator.Instance.ResolveType<IRegionManager>(); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Activates the content of the document.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="view"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="regionName"/> is <c>null</c> or whitespace.</exception>
        public static void ActivateDocumentContent(FrameworkElement view, string regionName)
        {
            Argument.IsNotNull("view", view);
            Argument.IsNotNullOrWhitespace("regionName", regionName);

            var layoutDocument = WrapViewInLayoutDocument(view);

            // TODO: Check if child already added, then only activate
            LayoutDocumentPane.Children.Add(layoutDocument);

            //var region = RegionManager.Regions[regionName];
            //view.Tag = regionName;

            //var document = region.Views.Cast<LayoutDocument>().FirstOrDefault(d => ((FrameworkElement)d.Content).Name == view.Name);
            //if (document != null)
            //{
            //    region.Activate(document);
            //}
            //else
            //{
            //    region.Add(view);
            //    region.Activate(view);
            //}
        }

        /// <summary>
        /// Wraps the view in a layout document.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>A wrapped layout document.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="view"/> is <c>null</c>.</exception>
        private static LayoutDocument WrapViewInLayoutDocument(FrameworkElement view)
        {
            Argument.IsNotNull("view", view);

            var layoutDocument = new LayoutDocument();

            layoutDocument.CanFloat = false;
            // TODO: Make bindable => automatic updates
            layoutDocument.Title = ((IViewModel) view.DataContext).Title;
            layoutDocument.Content = view;

            return layoutDocument;
        }

        /// <summary>
        /// Called when the docking manager has just closed a document.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="AvalonDock.DocumentClosedEventArgs"/> instance containing the event data.</param>
        private static void OnDockingManagerDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            var containerView = (LayoutDocument)sender;
            var view = (FrameworkElement)containerView.Content;
            var region = RegionManager.Regions[(string)view.Tag];

            region.Remove(sender);
        }
        #endregion
    }
}