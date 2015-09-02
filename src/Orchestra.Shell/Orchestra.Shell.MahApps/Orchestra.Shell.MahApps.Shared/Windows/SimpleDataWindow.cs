// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleDataWindow.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Catel;
    using Catel.MVVM;
    using Catel.MVVM.Providers;
    using Catel.MVVM.Views;
    using Catel.Threading;
    using Catel.Windows;

    public class SimpleDataWindow : MahApps.Metro.Controls.Dialogs.CustomDialog, IDataWindow
    {
        #region Fields
        private readonly WindowLogic _logic;

        private event EventHandler<EventArgs> _viewLoaded;
        private event EventHandler<EventArgs> _viewUnloaded;
        private event EventHandler<DataContextChangedEventArgs> _viewDataContextChanged;

        private readonly Collection<DataWindowButton> _buttons = new Collection<DataWindowButton>();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDataWindow"/> class.
        /// </summary>
        protected SimpleDataWindow()
            : this(DataWindowMode.OkCancel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDataWindow"/> class.
        /// </summary>
        protected SimpleDataWindow(DataWindowMode dataWindowMode)
            : this(null, dataWindowMode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDataWindow" /> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="mode">The data window mode.</param>
        /// <param name="additionalButtons">The additional buttons.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        protected SimpleDataWindow(IViewModel viewModel, DataWindowMode mode = DataWindowMode.OkCancel, IEnumerable<DataWindowButton> additionalButtons = null)
        {
            _logic = new WindowLogic(this, null, viewModel);
            _logic.PropertyChanged += (sender, e) => PropertyChanged.SafeInvoke(this, e);
            _logic.ViewModelChanged += (sender, e) => ViewModelChanged.SafeInvoke(this, e);
            _logic.ViewModelPropertyChanged += (sender, e) => ViewModelPropertyChanged.SafeInvoke(this, e);

            Loaded += (sender, e) => _viewLoaded.SafeInvoke(this);
            Unloaded += (sender, e) => _viewUnloaded.SafeInvoke(this);
            DataContextChanged += (sender, e) => _viewDataContextChanged.SafeInvoke(this, new DataContextChangedEventArgs(e.OldValue, e.NewValue));

            if (additionalButtons != null)
            {
                foreach (var button in additionalButtons)
                {
                    _buttons.Add(button);
                }
            }

            if (mode == DataWindowMode.OkCancel || mode == DataWindowMode.OkCancelApply)
            {
                var button = new DataWindowButton("Ok", async () => await OnOkExecuteAsync(), OnOkCanExecute);
                button.IsDefault = true;
                _buttons.Add(button);
            }

            if (mode == DataWindowMode.OkCancel || mode == DataWindowMode.OkCancelApply)
            {
                var button = new DataWindowButton("Cancel", async () => await OnCancelExecuteAsync(), OnCancelCanExecute);
                button.IsCancel = true;
                _buttons.Add(button);
            }

            if (mode == DataWindowMode.OkCancelApply)
            {
                var button = new DataWindowButton("Apply", OnApplyExcute, OnApplyCanExecute);
                _buttons.Add(button);
            }

            if (mode == DataWindowMode.Close)
            {
                var button = new DataWindowButton("Close", OnCloseExecute);
                _buttons.Add(button);
            }

            // Call manually the first time (for injected view models)
            OnViewModelChanged();

            this.FixBlurriness();
        }
        #endregion

        #region Commands
        /// <summary>
        /// Executes the Ok command.
        /// </summary>
        protected Task ExecuteOkAsync()
        {
            if (OnOkCanExecute())
            {
                return OnOkExecuteAsync();
            }

            return TaskHelper.Completed;
        }

        /// <summary>
        /// Determines whether the user can execute the Ok command.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c>.</returns>
        protected bool OnOkCanExecute()
        {
            return ValidateData();
        }

        /// <summary>
        /// Handled when the user invokes the Ok command.
        /// </summary>
        protected async Task OnOkExecuteAsync()
        {
            if (!await ApplyChangesAsync())
            {
                return;
            }

            Close();
        }

        /// <summary>
        /// Executes the Cancel command.
        /// </summary>
        protected Task ExecuteCancelAsync()
        {
            if (OnCancelCanExecute())
            {
                return OnCancelExecuteAsync();
            }

            return TaskHelper.Completed;
        }

        /// <summary>
        /// Determines whether the user can execute the Cancel command.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c>.</returns>
        protected bool OnCancelCanExecute()
        {
            return true;
        }

        /// <summary>
        /// Handled when the user invokes the Cancel command.
        /// </summary>
        protected async Task OnCancelExecuteAsync()
        {
            if (!await DiscardChangesAsync())
            {
                return;
            }

            Close();
        }

        /// <summary>
        /// Executes the Apply command.
        /// </summary>
        protected void ExecuteApply()
        {
            if (OnApplyCanExecute())
            {
                OnApplyExcute();
            }
        }

        /// <summary>
        /// Determines whether the user can execute the Apply command.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c>.</returns>
        protected bool OnApplyCanExecute()
        {
            return ValidateData();
        }

        /// <summary>
        /// Handled when the user invokes the Apply command.
        /// </summary>
        protected async void OnApplyExcute()
        {
            await ApplyChangesAsync();
        }

        /// <summary>
        /// Executes the Close command.
        /// </summary>
        protected void ExecuteClose()
        {
            OnCloseExecute();
        }

        /// <summary>
        /// Handled when the user invokes the Close command.
        /// </summary>
        protected void OnCloseExecute()
        {
            Close();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a custom button to the list of buttons.
        /// </summary>
        /// <param name="dataWindowButton">The data window button.</param>
        /// <exception cref="InvalidOperationException">The <paramref name="dataWindowButton"/> is added when the window is already loaded.</exception>
        protected void AddCustomButton(DataWindowButton dataWindowButton)
        {
            _buttons.Add(dataWindowButton);
        }

        /// <summary>
        /// Validates the data.
        /// </summary>
        /// <returns>True if successful, otherwise false.</returns>
        protected virtual bool ValidateData()
        {
            return _logic.ValidateViewModel();
        }

        /// <summary>
        /// Applies all changes made by this window.
        /// </summary>
        /// <returns>True if successful, otherwise false.</returns>
        protected virtual async Task<bool> ApplyChangesAsync()
        {
            return await _logic.SaveViewModelAsync();
        }

        /// <summary>
        /// Discards all changes made by this window.
        /// </summary>
        protected virtual async Task<bool> DiscardChangesAsync()
        {
            return await _logic.CancelViewModelAsync();
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

        /// <summary>
        /// Occurs when the <see cref="ViewModel"/> property has changed.
        /// </summary>
        public event EventHandler<EventArgs> ViewModelChanged;

        /// <summary>
        /// Occurs when a property on the <see cref="P:Catel.MVVM.IViewModelContainer.ViewModel" /> has changed.
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
        public void Close()
        {
            if (IsVisible)
            {
                this.Close(ParentDialogWindow);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Margin = new Thickness(12);
            stackPanel.HorizontalAlignment = HorizontalAlignment.Right;

            foreach (var button in _buttons)
            {
                var finalButton = new Button
                {
                    Content = button.Text,
                    Command = button.Command,
                    IsDefault = button.IsDefault,
                    IsCancel = button.IsCancel,
                    MinWidth = 125,
                    Height = 34,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(6, 12, 6, 12)
                };

                stackPanel.Children.Add(finalButton);
            }

            DialogBottom = stackPanel;
        }

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