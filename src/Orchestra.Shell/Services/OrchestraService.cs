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

        /// <summary>
        /// Initializes a new instance of the <see cref="OrchestraService" /> class.
        /// </summary>
        /// <param name="shell">The shell.</param>
        public OrchestraService(MainWindow shell)
        {
            Argument.IsNotNull("shell", shell);

            _shell = shell;
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
        /// Adds the specified ribbon item to the main ribbon.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void AddRibbonItem(IRibbonItem ribbonItem)
        {
            // Marked obsolete on the interface
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the specified ribbon item to the main ribbon.
        /// <para />
        /// This method will ignore calls when the item is not available in the ribbon.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RemoveRibbonItem(IRibbonItem ribbonItem)
        {
            // Marked obsolete on the interface
            throw new NotImplementedException();
        }
    }
}