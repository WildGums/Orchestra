// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewActivationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.MVVM.Views;

    public class ViewActivationService : IViewActivationService
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly IViewManager _viewManager;
        #endregion

        #region Constructors
        public ViewActivationService(IViewManager viewManager)
        {
            ArgumentNullException.ThrowIfNull(viewManager);

            _viewManager = viewManager;
        }
        #endregion

        #region Methods
        public bool Activate(IViewModel viewModel)
        {
            ArgumentNullException.ThrowIfNull(viewModel);

            return Activate(vm => ReferenceEquals(vm, viewModel));
        }

        public bool Activate(Type viewModelType)
        {
            ArgumentNullException.ThrowIfNull(viewModelType);

            return Activate(vm => vm.GetType() == viewModelType);
        }
        #endregion

        private bool Activate(Func<IViewModel, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            foreach (var view in _viewManager.ActiveViews)
            {
                var vm = view.ViewModel;
                if (vm is not null && predicate(vm))
                {
                    var userControl = view as UserControl;
                    if (userControl is not null)
                    {
                        Log.Debug("View already exists, activating existing instance");

                        userControl.Focus();
                        return true;
                    }

                    var window = view as Window;
                    if (window is not null)
                    {
                        Log.Debug("View already exists, activating existing instance");

                        window.Focus();
                        return true;
                    }
                }
            }

            Log.Debug("Existing view not found");

            return false;
        }
    }
}
