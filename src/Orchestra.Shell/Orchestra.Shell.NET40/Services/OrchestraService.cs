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
        /// Shows the helper window.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="tag">The tag.</param>
        public void ShowDocumentIfHidden<TViewModel>(object tag = null)
            where TViewModel : IViewModel
        {
            var viewLocator = GetService<IViewLocator>();
            var viewType = viewLocator.ResolveView(typeof (TViewModel));

            var document = AvalonDockHelper.FindDocument(viewType, tag);

            if (document != null)
            {
                document.Show();
                Log.Debug("Show hidden document '{0}'", document.Title);
            }

            Log.Debug("Can not show hidden view because it can not be found for viewmodel type {0}", typeof(TViewModel));
        }

        /// <summary>
        /// Shows the document in the main shell.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="tag">The tag.</param>
        public void ShowDocument<TViewModel>(object tag = null)
            where TViewModel : IViewModel
        {
            var viewModel = CreateViewModel<TViewModel>();

            ShowDocument(viewModel, tag);
        }

        /// <summary>
        /// Opens a new document.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="viewModel">The view model.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="dockingSettings">The docking settings.</param>
        public void ShowDocument<TViewModel>(TViewModel viewModel, object tag = null, DockingSettings dockingSettings = null)
            where TViewModel : IViewModel
        {
            Argument.IsNotNull("viewModel", viewModel);
            Log.Debug("Opening document for view model '{0}'", viewModel.UniqueIdentifier);

            var document = CreateDocument(viewModel, tag);
            AvalonDockHelper.AddNewDocumentToDockingManager(dockingSettings, document);
            AvalonDockHelper.ActivateDocument(document);
        }        

        /// <summary>
        /// Shows the document in the main shell.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="tag">The tag.</param>
        public void ShowContextSensitiveDocument<TViewModel>(object tag = null)
            where TViewModel : IViewModel
        {
            var viewModel = CreateViewModel<TViewModel>();
            ShowContextSensitiveDocument(viewModel, tag);
        }

        /// <summary>
        /// Shows the document in the main shell.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="viewModel">The view model to show which will automatically be resolved to a view.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="dockingSettings">The docking settings.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="viewModel" /> is <c>null</c>.</exception>
        public void ShowContextSensitiveDocument<TViewModel>(TViewModel viewModel, object tag = null, DockingSettings dockingSettings = null) 
            where TViewModel : IViewModel
        {
            Argument.IsNotNull("viewModel", viewModel);

            Log.Debug("Showing document for view model '{0}'", viewModel.UniqueIdentifier);

            var viewType = GetViewType(viewModel);

            var document = AvalonDockHelper.FindDocument(viewType, tag);
            if (document == null)
            {
                var view = ViewHelper.ConstructViewWithViewModel(viewType, viewModel);
                document = AvalonDockHelper.CreateDocument(view, tag);
                if (dockingSettings != null)
                {
                    AvalonDockHelper.AddNewDocumentToDockingManager(dockingSettings, document);
                }
            }            

            AvalonDockHelper.ActivateDocument(document);

            Log.Debug("Showed document for view model '{0}'", viewModel.UniqueIdentifier);
        }

        /// <summary>
        /// Shows the document in nested dock view.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="dockingManager">The docking manager.</param>
        /// <param name="dockingSettings">The docking settings.</param>
        /// <param name="tag">The tag.</param>
        public void ShowDocumentInNestedDockView(IViewModel viewModel, NestedDockingManager dockingManager, DockingSettings dockingSettings, object tag = null)
        {
            var document = CreateDocument(viewModel, tag);            
            dockingManager.AddDockedWindow(document, dockingSettings);
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

            var viewType = GetViewType(viewModel);
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

        private LayoutAnchorable CreateDocument<TViewModel>(TViewModel viewModel, object tag) where TViewModel : IViewModel
        {
            var viewLocator = GetService<IViewLocator>();
            var viewType = viewLocator.ResolveView(viewModel.GetType());
            var view = ViewHelper.ConstructViewWithViewModel(viewType, viewModel);
            var document = AvalonDockHelper.CreateDocument(view, tag);
            return document;
        }

        private static TViewModel CreateViewModel<TViewModel>() where TViewModel : IViewModel
        {
            var typeFactory = TypeFactory.Default;
            var viewModel = typeFactory.CreateInstance<TViewModel>();
            return viewModel;
        }

        private Type GetViewType<TViewModel>(TViewModel viewModel) where TViewModel : IViewModel
        {
            Argument.IsNotNull(() => viewModel);

            var viewLocator = GetService<IViewLocator>();
            var viewType = viewLocator.ResolveView(viewModel.GetType());
            return viewType;
        }
        #endregion
    }
}