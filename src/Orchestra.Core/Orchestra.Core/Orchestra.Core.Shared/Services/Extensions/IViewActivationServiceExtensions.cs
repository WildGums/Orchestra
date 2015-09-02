// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewActivationServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;

    public static class IViewActivationServiceExtensions
    {
        public static void ActivateOrShow<TViewModel>(this IViewActivationService viewActivationService)
            where TViewModel : IViewModel
        {
            viewActivationService.ActivateOrShow(typeof (TViewModel));
        }

        public static void ActivateOrShow(this IViewActivationService viewActivationService, Type viewModelType)
        {
            Argument.IsNotNull(() => viewActivationService);
            Argument.IsNotNull(() => viewModelType);

            if (!viewActivationService.Activate(viewModelType))
            {
                var dependencyResolver = viewActivationService.GetDependencyResolver();

                var viewModelFactory = dependencyResolver.Resolve<IViewModelFactory>();
                var uiVisualizerService = dependencyResolver.Resolve<IUIVisualizerService>();

                var vm = viewModelFactory.CreateViewModel(viewModelType, null);
                uiVisualizerService.Show(vm);
            }
        }

        public static void ActivateOrShow(this IViewActivationService viewActivationService, IViewModel viewModel)
        {
            Argument.IsNotNull(() => viewActivationService);
            Argument.IsNotNull(() => viewModel);

            if (!viewActivationService.Activate(viewModel))
            {
                var dependencyResolver = viewActivationService.GetDependencyResolver();

                var uiVisualizerService = dependencyResolver.Resolve<IUIVisualizerService>();

                uiVisualizerService.Show(viewModel);
            }
        }
    }
}