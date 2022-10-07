namespace Orchestra.Services
{
    using System;
    using System.Threading.Tasks;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;

    public static class IViewActivationServiceExtensions
    {
        public static Task ActivateOrShowAsync<TViewModel>(this IViewActivationService viewActivationService)
            where TViewModel : IViewModel
        {
            ArgumentNullException.ThrowIfNull(viewActivationService);

            return viewActivationService.ActivateOrShowAsync(typeof (TViewModel));
        }

        public static async Task ActivateOrShowAsync(this IViewActivationService viewActivationService, Type viewModelType)
        {
            ArgumentNullException.ThrowIfNull(viewActivationService);
            ArgumentNullException.ThrowIfNull(viewModelType);

            if (!viewActivationService.Activate(viewModelType))
            {
                var dependencyResolver = viewActivationService.GetDependencyResolver();

                var viewModelFactory = dependencyResolver.ResolveRequired<IViewModelFactory>();
                var uiVisualizerService = dependencyResolver.ResolveRequired<IUIVisualizerService>();

                var vm = viewModelFactory.CreateRequiredViewModel(viewModelType, null, null);
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

                var uiVisualizerService = dependencyResolver.ResolveRequired<IUIVisualizerService>();

                await uiVisualizerService.ShowAsync(viewModel);
            }
        }
    }
}
