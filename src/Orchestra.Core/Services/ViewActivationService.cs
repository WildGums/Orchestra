namespace Orchestra.Services
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Catel.MVVM;
    using Catel.MVVM.Views;
    using Microsoft.Extensions.Logging;

    public class ViewActivationService : IViewActivationService
    {
        private readonly ILogger<ViewActivationService> _logger;
        private readonly IViewManager _viewManager;

        public ViewActivationService(ILogger<ViewActivationService> logger, IViewManager viewManager)
        {
            _logger = logger;
            _viewManager = viewManager;
        }

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
                        _logger.LogDebug("View already exists, activating existing instance");

                        userControl.Focus();
                        return true;
                    }

                    var window = view as Window;
                    if (window is not null)
                    {
                        _logger.LogDebug("View already exists, activating existing instance");

                        window.Focus();
                        return true;
                    }
                }
            }

            _logger.LogDebug("Existing view not found");

            return false;
        }
    }
}
