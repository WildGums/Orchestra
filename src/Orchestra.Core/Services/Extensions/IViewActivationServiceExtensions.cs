namespace Orchestra;

using System;
using System.Threading.Tasks;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using Microsoft.Extensions.DependencyInjection;

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
            var serviceProvider = IoCContainer.ServiceProvider;

            var viewModelFactory = serviceProvider.GetRequiredService<IViewModelFactory>();
            var uiVisualizerService = serviceProvider.GetRequiredService<IUIVisualizerService>();

            var vm = viewModelFactory.CreateRequiredViewModel(viewModelType, null);
            await uiVisualizerService.ShowAsync(vm);
        }
    }

    public static async Task ActivateOrShowAsync(this IViewActivationService viewActivationService, IViewModel viewModel)
    {
        ArgumentNullException.ThrowIfNull(viewActivationService);
        ArgumentNullException.ThrowIfNull(viewModel);

        if (!viewActivationService.Activate(viewModel))
        {
            var serviceProvider = IoCContainer.ServiceProvider;

            var uiVisualizerService = serviceProvider.GetRequiredService<IUIVisualizerService>();

            await uiVisualizerService.ShowAsync(viewModel);
        }
    }
}
