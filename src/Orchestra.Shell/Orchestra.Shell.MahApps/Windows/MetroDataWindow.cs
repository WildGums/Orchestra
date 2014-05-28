// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataMetroWindow.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Windows
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.MVVM.Providers;
    using Catel.MVVM.Views;
    using Catel.Windows;
    using MahApps.Metro.Behaviours;
    using MahApps.Metro.Controls;

    /// <summary>
    /// Base class for a metro window with the Catel mvvm behavior.
    /// </summary>
    public abstract class MetroDataWindow : MetroWindow, IDataWindow
    {
        #region Fields
        private readonly WindowLogic _logic;

        private event EventHandler<EventArgs> _viewLoaded;
        private event EventHandler<EventArgs> _viewUnloaded;
        private event EventHandler<EventArgs> _viewDataContextChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MetroDataWindow"/> class.
        /// </summary>
        protected MetroDataWindow()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetroDataWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        protected MetroDataWindow(IViewModel viewModel)
        {
            var viewModelType = (viewModel != null) ? viewModel.GetType() : GetViewModelType();
            if (viewModelType == null)
            {
                var viewModelLocator = ServiceLocator.Default.ResolveType<IViewModelLocator>();
                viewModelType = viewModelLocator.ResolveViewModel(GetType());
                if (viewModelType == null)
                {
                    const string error = "The view model of the view could not be resolved. Use either the GetViewModelType() method or IViewModelLocator";
                    throw new NotSupportedException(error);
                }
            }

            _logic = new WindowLogic(this, viewModelType, viewModel);
            _logic.ViewModelChanged += (sender, e) => ViewModelChanged.SafeInvoke(this, e);
            _logic.ViewModelPropertyChanged += (sender, e) => ViewModelPropertyChanged.SafeInvoke(this, e);
            _logic.PropertyChanged += (sender, e) => PropertyChanged.SafeInvoke(this, e);
            _logic.ViewLoading += (sender, e) => ViewLoading.SafeInvoke(this);
            _logic.ViewLoaded += (sender, e) => ViewLoaded.SafeInvoke(this);
            _logic.ViewUnloading += (sender, e) => ViewUnloading.SafeInvoke(this);
            _logic.ViewUnloaded += (sender, e) => ViewUnloaded.SafeInvoke(this);

            Loaded += (sender, e) => _viewLoaded.SafeInvoke(this);
            Unloaded += (sender, e) => _viewUnloaded.SafeInvoke(this);
            this.AddDataContextChangedHandler((sender, e) => _viewDataContextChanged.SafeInvoke(this));

            // Because the RadWindow does not close when DialogResult is set, the following code is required
            ViewModelChanged += (sender, e) => OnViewModelChanged();

            // Call manually the first time (for injected view models)
            OnViewModelChanged();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            this.ApplyIconFromApplication();

            // Since we customize behaviors, we need to add the default MahApps behaviors as well
            this.ApplyBehavior<BorderlessWindowBehavior>();
            this.ApplyBehavior<WindowsSettingBehaviour>();
            this.ApplyBehavior<GlowWindowBehavior>();

            AllowsTransparency = true;
            EnableDWMDropShadow = true;
        }
        #endregion

        #region IDataWindow Members
        /// <summary>
        /// Gets the view model that is contained by the container.
        /// </summary>
        /// <value>The view model.</value>
        public IViewModel ViewModel
        {
            get { return _logic.ViewModel; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the view model container should prevent the creation of a view model.
        /// <para />
        /// This property is very useful when using views in transitions where the view model is no longer required.
        /// </summary>
        /// <value><c>true</c> if the view model container should prevent view model creation; otherwise, <c>false</c>.</value>
        public bool PreventViewModelCreation
        {
            get { return _logic.PreventViewModelCreation; }
            set { _logic.PreventViewModelCreation = value; }
        }

        /// <summary>
        /// Gets the logical parent  element of this element.
        /// </summary>
        /// <value>The parent.</value>
        /// <returns>This element's logical parent.</returns>
        object IView.Parent
        {
            get { return null; }
        }

        /// <summary>
        /// Occurs when the <see cref="ViewModel"/> property has changed.
        /// </summary>
        public event EventHandler<EventArgs> ViewModelChanged;

        /// <summary>
        /// Occurs when the view model container is loading.
        /// </summary>
        public event EventHandler<EventArgs> ViewLoading;

        /// <summary>
        /// Occurs when the view model container is loaded.
        /// </summary>
        public event EventHandler<EventArgs> ViewLoaded;

        /// <summary>
        /// Occurs when the view model container starts unloading.
        /// </summary>
        public event EventHandler<EventArgs> ViewUnloading;

        /// <summary>
        /// Occurs when the view model container is unloaded.
        /// </summary>
        public event EventHandler<EventArgs> ViewUnloaded;

        /// <summary>
        /// Occurs when the view is loaded.
        /// </summary>
        event EventHandler<EventArgs> IView.Loaded
        {
            add { _viewLoaded += value; }
            remove { _viewLoaded -= value; }
        }

        /// <summary>
        /// Occurs when the view is unloaded.
        /// </summary>
        event EventHandler<EventArgs> IView.Unloaded
        {
            add { _viewUnloaded += value; }
            remove { _viewUnloaded -= value; }
        }

        /// <summary>
        /// Occurs when the data context has changed.
        /// </summary>
        event EventHandler<EventArgs> IView.DataContextChanged
        {
            add { _viewDataContextChanged += value; }
            remove { _viewDataContextChanged -= value; }
        }

        /// <summary>
        /// Occurs when a property on the <see cref="ViewModel"/> has changed.
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs> ViewModelPropertyChanged;

        /// <summary>
        /// Occurs when a property on the container has changed.
        /// </summary>
        /// <remarks>
        /// This event makes it possible to externally subscribe to property changes of a <see cref="DependencyObject"/>
        /// (mostly the container of a view model) because the .NET Framework does not allows us to.
        /// </remarks>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        protected virtual Type GetViewModelType()
        {
            return null;
        }

        private void OnViewModelChanged()
        {
            if (ViewModel != null && !ViewModel.IsClosed)
            {
                ViewModel.Closed += ViewModelClosed;
            }
        }

        private void ViewModelClosed(object sender, ViewModelClosedEventArgs e)
        {
            Close();
        }
        #endregion
    }
}