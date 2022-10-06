// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewActivationServiceExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;

    public static class IViewActivationServiceExtensions
    {
        public static Task ActivateOrShowAsync<TViewModel>(this IViewActivationService viewActivationService)
            where TViewModel : IViewModel
        {
            return viewActivationService.ActivateOrShowAsync(typeof (TViewModel));
        }

        public static async Task ActivateOrShowAsync(this IViewActivationService viewActivationService, Type viewModelType)
        {
            ArgumentNullException.ThrowIfNull(viewActivationService);
            ArgumentNullException.ThrowIfNull(viewModelType);

            if (!viewActivationService.Activate(viewModelType))
            {
                var dependencyResolver = viewActivationService.GetDependencyResolver();

                var viewModelFactory = dependencyResolver.Resolve<IViewModelFactory>();
                var uiVisualizerService = dependencyResolver.Resolve<IUIVisualizerService>();

                var vm = viewModelFactory.CreateViewModel(viewModelType, null, null);
                await uiVisualizerService.ShowAsync(vm);
            }
        }

        public static async Task ActivateOrShowAsync(this IViewActivationService viewActivationService, IViewModel viewModel)
        {
            ArgumentNullException.ThrowIfNull(viewActivationService);
            ArgumentNullException.ThrowIfNull(viewModel);

            if (!viewActivationService.Activate(viewModel))
            {
                var dependencyResolver = viewActivationService.GetDependencyResolver();

                var uiVisualizerService = dependencyResolver.Resolve<IUIVisualizerService>();

                await uiVisualizerService.ShowAsync(viewModel);
            }
        }
    }
}