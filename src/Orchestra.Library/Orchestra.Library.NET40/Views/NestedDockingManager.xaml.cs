// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NestedDockingManager.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orchestra.Views
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using Catel;
    using Models;
    using Xceed.Wpf.AvalonDock;
    using Xceed.Wpf.AvalonDock.Layout;

    /// <summary>
    /// Interaction logic for NestedDockingManager.xaml
    /// </summary>
    public partial class NestedDockingManager : IDockingManagerContainer
    {
        private Dictionary<LayoutAnchorable, DockingSettings> _floatingWindowsCollection = new Dictionary<LayoutAnchorable, DockingSettings>(); 

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
        /// <param name="dockingSettings">The docking settings.</param>
        public void AddDockedWindow(LayoutAnchorable documentView, DockingSettings dockingSettings)
        {
            Argument.IsNotNull(() => documentView);
            Argument.IsNotNull(() => dockingSettings);            

            switch (dockingSettings.DockLocation)
            {
                case DockLocation.Right:
                    rightPropertiesPane.Children.Add(documentView);
                    SetDockWidthForPane(rightPropertiesPane, dockingSettings);
                    break;
                case DockLocation.Left:
                    leftPropertiesPane.Children.Add(documentView);
                    SetDockWidthForPane(leftPropertiesPane, dockingSettings);
                    break;
                case DockLocation.Bottom:
                    bottomPropertiesPane.Children.Add(documentView);                    
                    break;
                case DockLocation.Top:
                    topPropertiesPane.Children.Add(documentView);                    
                    break;
                //case DockLocation.Floating:
                //    rightPropertiesPane.Children.Add(documentView);                    
                //    _floatingWindowsCollection.Add(documentView, dockingSettings);
                //    break;
            }            
        }

        /// <summary>
        /// Handles the Loaded event of the NestedDockingManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void NestedDockingManagerLoaded(object sender, RoutedEventArgs e)
        {
            // Forwared the 'focus' to the correct view.
            DockingManager.ActiveContent = ContentDocument.Content;
            DelayedOpenFloatingWindows();
        }

        /// <summary>
        /// Opens the registered floating windows for this module, the opening of these windows is delayed to when this view is actually loaded.        
        /// </summary>
        private void DelayedOpenFloatingWindows()
        {
            if (_floatingWindowsCollection == null)
            {
                return;
            }

            foreach (var document in _floatingWindowsCollection.Keys)
            {
                document.Float();

                SetWindowSize(document);
            }

            _floatingWindowsCollection = null;
        }

        /// <summary>
        /// Sets the size of the window.
        /// </summary>
        /// <param name="document">The document.</param>
        private void SetWindowSize(LayoutAnchorable document)
        {
            foreach (var window in dockingManager.FloatingWindows)
            {
                var model = window.Model as LayoutAnchorableFloatingWindow;

                if (model != null)
                {
                    foreach (var child in model.SinglePane.Children)
                    {
                        if (child == document)
                        {
                            window.Top = _floatingWindowsCollection[document].Top;
                            window.Left = _floatingWindowsCollection[document].Left;
                            window.Width = _floatingWindowsCollection[document].Width;
                            window.Width = _floatingWindowsCollection[document].Height;
                        }
                    }
                }
            }
        }

        private void SetDockWidthForPane(LayoutAnchorablePane propertiesPane, DockingSettings dockingSettings)
        {            
            Argument.IsNotNull(()=>propertiesPane);
            Argument.IsNotNull(() => dockingSettings);

            var paneGroup = propertiesPane.Parent as LayoutAnchorablePaneGroup;            

            if (paneGroup != null && paneGroup.DockWidth.Value < dockingSettings.Width)
            {
                // There is an issue in AvalonDock: https://avalondock.codeplex.com/workitem/16427
                // When a Document is Opened in a NestedDockingManager from the menu, the set size for the width is reset by AvalonDock to Star size.
                // This is a workaround to get arround this issue.
                paneGroup.PropertyChanged += PaneGroupPropertyChanged;

                paneGroup.DockWidth = new GridLength(dockingSettings.Width);
                _dockWidth = dockingSettings.Width;
            }
        }
        #region Workaround for star sizing issue
        // We remeber the dockWidth that is set. So when the size is > 0, we know it has been set by this instance.
        private int _dockWidth;

        /// <summary>
        /// Handles the PropertyChanged event of the paneGroup control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        void PaneGroupPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var paneGroup = sender as LayoutAnchorablePaneGroup;

            if (e.PropertyName == "DockWidth" && paneGroup != null && paneGroup.DockWidth.IsStar && _dockWidth > 0)
            {                
                paneGroup.PropertyChanged -= PaneGroupPropertyChanged;
                paneGroup.DockWidth = new GridLength(_dockWidth);                
            }
        }
        #endregion

        #endregion
    }
}