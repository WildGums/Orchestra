// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewActivationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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
            Argument.IsNotNull(() => viewManager);

            _viewManager = viewManager;
        }
        #endregion

        #region Methods
        public bool Activate(IViewModel viewModel)
        {
            Argument.IsNotNull(() => viewModel);

            return Activate(vm => ReferenceEquals(vm, viewModel));
        }

        public bool Activate(Type viewModelType)
        {
            Argument.IsNotNull(() => viewModelType);

            return Activate(vm => vm.GetType() == viewModelType);
        }
        #endregion

        private bool Activate(Func<IViewModel, bool> predicate)
        {
            Argument.IsNotNull(() => predicate);

            foreach (var view in _viewManager.ActiveViews)
            {
                var vm = view.ViewModel;
                if (vm != null)
                {
                    if (predicate(vm))
                    {
                        var userControl = view as UserControl;
                        if (userControl != null)
                        {
                            Log.Info("View already exists, activating existing instance");

                            userControl.Focus();
                            return true;
                        }

                        var window = view as Window;
                        if (window != null)
                        {
                            Log.Info("View already exists, activating existing instance");

                            window.Focus();
                            return true;
                        }
                    }
                }
            }

            Log.Info("Existing view not found");

            return false;
        }
    }
}