// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrchestraService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orchestra.Services
{
    using System;

    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Windows.Threading;

    using Orchestra.Models;
    using Orchestra.Views;
    using Xceed.Wpf.AvalonDock.Layout;

    /// <summary>
    /// Orchestra service implementation.
    /// </summary>
    public class OrchestraService : ServiceBase, IOrchestraService
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly MainWindow _shell;

        private bool _showDebuggingWindow;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="OrchestraService" /> class.
        /// </summary>
        /// <param name="shell">The shell.</param>
        public OrchestraService(MainWindow shell)
        {
            Argument.IsNotNull("shell", shell);

            _shell = shell;
            _shell.Loaded += (sender, e) =>
            {
                if (ShowDebuggingWindow)
                {
                    _shell.Dispatcher.BeginInvoke(() => _shell.ShowAnchorable(MainWindow.TraceOutputAnchorable));
                }
            };
        }
        #endregion

        #region IOrchestraService Members
        /// <summary>
        /// Gets or sets a value indicating whether to show the debug window.
        /// </summary>
        /// <value><c>true</c> if the debug window should be shown; otherwise, <c>false</c>.</value>
        public bool ShowDebuggingWindow
        {
            get
            {
                return _showDebuggingWindow;
            }
            set
            {
                _showDebuggingWindow = value;

                try
                {
                    if (_showDebuggingWindow)
                    {
                        _shell.ShowAnchorable(MainWindow.TraceOutputAnchorable);
                    }
                    else
                    {
                        _shell.HideAnchorable(MainWindow.TraceOutputAnchorable);
                    }
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Failed to show/hide the debugging window");
                }
            }
        }

        /// <summary>
        /// Shows the document in the main shell.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="tag">The tag.</param>
        public void ShowDocument<TViewModel>(object tag = null) 
            where TViewModel : IViewModel
        {
            var typeFactory = TypeFactory.Default;
            var viewModel = typeFactory.CreateInstance<TViewModel>();

            ShowDocument(viewModel, tag);
        }

        /// <summary>
        /// Shows the document in the main shell.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="viewModel">The view model to show which will automatically be resolved to a view.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="dockLocation">The dock location.</param>
        /// <param name="contextualParentViewModel">The contextual parent view model.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="viewModel" /> is <c>null</c>.</exception>
        public void ShowDocument<TViewModel>(TViewModel viewModel, object tag = null, DockLocation? dockLocation = null, Orchestra.ViewModels.ViewModelBase contextualParentViewModel = null) 
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
                document = AvalonDockHelper.CreateDocument(view, tag, dockLocation, contextualParentViewModel);
            }            

            AvalonDockHelper.ActivateDocument(document);

            Log.Debug("Showed document for view model '{0}'", viewModel.UniqueIdentifier);
        }

        /// <summary>
        /// Closes the document in the main shell with the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="tag">The tag.</param>
        public void CloseDocument(IViewModel viewModel, object tag = null)
        {
            Argument.IsNotNull(() => viewModel);

            Log.Debug("Closing document for view model '{0}'", viewModel.UniqueIdentifier);

            var viewLocator = GetService<IViewLocator>();
            var viewType = viewLocator.ResolveView(viewModel.GetType());

            var document = AvalonDockHelper.FindDocument(viewType, tag);
            if (document == null)
            {
                Log.Warning("Cannot find document belonging to view model '{0}' with id '{1}' thus cannot close the document",
                    ObjectToStringHelper.ToTypeString(viewModel), viewModel.UniqueIdentifier);
            }
            else
            {
                AvalonDockHelper.CloseDocument(document);    
            }

            Log.Debug("Closed document for view model '{0}'", viewModel.UniqueIdentifier);
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
        /// <para>
        /// </para>
        /// This method will ignore calls when the item is not available in the ribbon.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RemoveRibbonItem(IRibbonItem ribbonItem)
        {
            // Marked obsolete on the interface
            throw new NotImplementedException();
        }
        #endregion
    }
}