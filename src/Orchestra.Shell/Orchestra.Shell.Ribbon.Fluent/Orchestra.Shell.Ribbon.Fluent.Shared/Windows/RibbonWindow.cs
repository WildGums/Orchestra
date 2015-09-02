// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonWindow.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Windows
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.MVVM;
    using Catel.MVVM.Providers;
    using Catel.MVVM.Views;
    using Catel.Windows;

    public class RibbonWindow : Fluent.RibbonWindow, IDataWindow
    {
        private readonly WindowLogic _logic;

        private event EventHandler<EventArgs> _viewLoaded;
        private event EventHandler<EventArgs> _viewUnloaded;
        private event EventHandler<DataContextChangedEventArgs> _viewDataContextChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonWindow"/> class.
        /// </summary>
        public RibbonWindow()
            : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public RibbonWindow(IViewModel viewModel)
        {
            _logic = new WindowLogic(this, null, viewModel);
            _logic.ViewModelChanged += (sender, e) => ViewModelChanged.SafeInvoke(this, e);
            _logic.PropertyChanged += (sender, e) => PropertyChanged.SafeInvoke(this, e);

            Loaded += (sender, e) => _viewLoaded.SafeInvoke(this);
            Unloaded += (sender, e) => _viewUnloaded.SafeInvoke(this);
            DataContextChanged += (sender, e) => _viewDataContextChanged.SafeInvoke(this, new DataContextChangedEventArgs(e.OldValue, e.NewValue));

            // Because the RadWindow does not close when DialogResult is set, the following code is required
            ViewModelChanged += (sender, e) => OnViewModelChanged();

            // Call manually the first time (for injected view models)
            OnViewModelChanged();

            this.FixBlurriness();
        }

        #region Properties
        /// <summary>
        /// Gets the view model that is contained by the container.
        /// </summary>
        /// <value>The view model.</value>
        public IViewModel ViewModel
        {
            get { return _logic.ViewModel; }
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the <see cref="ViewModel"/> property has changed.
        /// </summary>
        public event EventHandler<EventArgs> ViewModelChanged;

        /// <summary>
        /// Occurs when a property on the container has changed.
        /// </summary>
        /// <remarks>
        /// This event makes it possible to externally subscribe to property changes of a <see cref="DependencyObject"/>
        /// (mostly the container of a view model) because the .NET Framework does not allows us to.
        /// </remarks>
        public event PropertyChangedEventHandler PropertyChanged;

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
        event EventHandler<DataContextChangedEventArgs> IView.DataContextChanged
        {
            add { _viewDataContextChanged += value; }
            remove { _viewDataContextChanged -= value; }
        }
        #endregion

        #region Methods
        private void OnViewModelChanged()
        {
            if (ViewModel != null && !ViewModel.IsClosed)
            {
                ViewModel.ClosedAsync += ViewModelClosedAsync;
            }
        }

        private async Task ViewModelClosedAsync(object sender, ViewModelClosedEventArgs e)
        {
            Close();
        }
        #endregion
    }
}
