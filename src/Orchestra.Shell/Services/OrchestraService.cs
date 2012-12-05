// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrchestraService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Services
{
    using System;
    using System.ComponentModel;
    using System.Windows.Input;
    using AvalonDock.Layout;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Fluent;
    using Models;
    using Views;

    /// <summary>
    /// Orchestra service implementation.
    /// </summary>
    public class OrchestraService : ServiceBase, IOrchestraService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly MainWindow _shell;

        private readonly LayoutDocumentPane _layoutDocumentPane;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="OrchestraService"/> class.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="layoutDocumentPane">The layout document pane</param>
        /// <exception cref="ArgumentNullException">The <paramref name="shell"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="layoutDocumentPane"/> is <c>null</c>.</exception>
        public OrchestraService(MainWindow shell, LayoutDocumentPane layoutDocumentPane)
        {
            Argument.IsNotNull("shell", shell);
            Argument.IsNotNull("layoutDocumentPane", layoutDocumentPane);

            _shell = shell;
            _layoutDocumentPane.PropertyChanged += OnLayoutDocumentPanePropertyChange;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the debug window.
        /// </summary>
        /// <value><c>true</c> if the debug window should be shown; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool ShowDebuggingWindow
        {
            get { return _shell.IsAnchorableVisible(MainWindow.TraceOutputAnchorable); }
            set
            {
                if (value)
                {
                    _shell.ShowAnchorable(MainWindow.TraceOutputAnchorable);
                }
                else
                {
                    _shell.ShowAnchorable(MainWindow.TraceOutputAnchorable);
                }
            }
        }

        /// <summary>
        /// Shows the document in the main shell.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="tag">The tag.</param>
        public void ShowDocument<TViewModel>(object tag = null) 
            where TViewModel : IViewModel, new()
        {
            var viewModel = new TViewModel();

            ShowDocument(viewModel, tag);
        }

        /// <summary>
        /// Shows the document in the main shell.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="viewModel">The view model to show which will automatically be resolved to a view.</param>
        /// <param name="tag">The tag.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="viewModel"/> is <c>null</c>.</exception>
        public void ShowDocument<TViewModel>(TViewModel viewModel, object tag = null) 
            where TViewModel : IViewModel
        {
            Argument.IsNotNull("viewModel", viewModel);

            Log.Debug("Showing document for view model '{0}'", viewModel.UniqueIdentifier);

            var viewLocator = GetService<IViewLocator>();
            var viewType = viewLocator.ResolveView(viewModel.GetType());

            var document = AvalonDockHelper.FindDocument(viewType, tag);
            if (document == null)
            {
                var view = ViewHelper.ConstructViewWithViewModel(viewType, viewModel);
                document = AvalonDockHelper.CreateDocument(view, tag);
            }

            AvalonDockHelper.ActivateDocument(document);

            Log.Debug("Showed document for view model '{0}'", viewModel.UniqueIdentifier);
        }

        /// <summary>
        /// Adds a new ribbon item to the main ribbon.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        public void AddRibbonItem(IRibbonItem ribbonItem)
        {
            Argument.IsNotNull("ribbonItem", ribbonItem);

            Log.Debug("Adding ribbon item '{0}'", ribbonItem);

            var ribbon = GetService<Ribbon>();

            var tab = ribbon.EnsureTabItem(ribbonItem.TabItemHeader);
            var group = tab.EnsureGroupBox(ribbonItem.GroupBoxHeader);
            group.AddButton(ribbonItem.ItemHeader, ribbonItem.ItemImage, ribbonItem.ItemImage, ribbonItem.Command);

            Log.Debug("Added ribbon item '{0}'", ribbonItem);
        }

        /// <summary>
        /// Removes the specified ribbon item to the main ribbon.
        /// <para />
        /// This method will ignore calls when the item is not available in the ribbon.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        public void RemoveRibbonItem(IRibbonItem ribbonItem)
        {
            Argument.IsNotNull("ribbonItem", ribbonItem);

            Log.Debug("Removing ribbon '{0}'", ribbonItem);

            var ribbon = GetService<Ribbon>();
            ribbon.RemoveItem(ribbonItem);

            Log.Debug("Removed ribbon '{0}'", ribbonItem);
        }

        private void OnLayoutDocumentPanePropertyChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "SelectedContent"))
            {
                Log.Debug("SelectedContent changed, activating the right ribbon");

                var ribbon = GetService<Ribbon>();

                var documentView = _layoutDocumentPane.SelectedContent as IDocumentView;
                if (documentView == null)
                {
                    Log.Debug("SelectecContent is not a document view, selecting home ribbon item");

                    ribbon.SelectedTabIndex = 0;
                    return;
                }

                //var documentViewRibbon = documentView.

                // TODO: Find ribbon item, select it

                // TODO: If no ribbon item is available, select home

            }
        }
    }
}