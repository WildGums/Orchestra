// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NestedDockingManager.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using System;
    using Catel;
    using Orchestra.Models;
    using Xceed.Wpf.AvalonDock;
    using Xceed.Wpf.AvalonDock.Layout;

    /// <summary>
    /// Interaction logic for NestedDockingManager.xaml
    /// </summary>
    public partial class NestedDockingManager : IDockingManagerContainer
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NestedDockingManager"/> class.
        /// </summary>
        public NestedDockingManager()
        {
            InitializeComponent();
            Loaded += NestedDockingManagerLoaded;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="DockingManager" />.
        /// </summary>
        /// <value>
        /// The <see cref="DockingManager" />.
        /// </value>
        public DockingManager DockingManager
        {
            get { return dockingManager; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive
        {
            get { return IsVisible; }
        }

        /// <summary>
        /// Gets the content document.
        /// </summary>
        /// <value>
        /// The content document.
        /// </value>
        public LayoutAnchorable ContentDocument
        {
            get
            {
                if (layoutDocumentPane.Children.Count > 0)
                {
                    return layoutDocumentPane.Children[0];
                }

                return null;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds the main document for the NestedDockingManager.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        /// <exception cref="System.Exception">NestedDockingManager can only have one main document with the main content.</exception>
        public void AddContentDocument(LayoutAnchorable documentView)
        {
            Argument.IsNotNull(() => documentView);

            if (layoutDocumentPane.Children.Count > 0)
            {
                throw new Exception("NestedDockingManager can only have one main document.");
            }

            layoutDocumentPane.Children.Add(documentView);
        }

        /// <summary>
        /// Adds the document as a tab to the NestedDocking view.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        /// <param name="dockLocation">The dock location.</param>
        public void AddDockedWindow(LayoutAnchorable documentView, DockLocation dockLocation)
        {
            Argument.IsNotNull(() => documentView);
            Argument.IsNotNull(() => dockLocation);

            if (dockLocation == DockLocation.Right)
            {
                rightPropertiesPane.Children.Add(documentView);
            }
            else if (dockLocation == DockLocation.Left)
            {
                leftPropertiesPane.Children.Add(documentView);
            }
            else if (dockLocation == DockLocation.Bottom)
            {
                bottomPropertiesPane.Children.Add(documentView);
            }
            else
            {
                topPropertiesPane.Children.Add(documentView);
            }
        }

        /// <summary>
        /// Handles the Loaded event of the NestedDockingManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void NestedDockingManagerLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Forwared the 'focus' to the correct view.
            DockingManager.ActiveContent = ContentDocument.Content;
        }
        #endregion
    }
}