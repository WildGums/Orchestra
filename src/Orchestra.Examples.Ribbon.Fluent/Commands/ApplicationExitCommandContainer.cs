namespace Orchestra.Examples.Ribbon;

using System;
using System.Threading.Tasks;
using Catel.MVVM;
using Catel.Services;
using Microsoft.Extensions.DependencyInjection;

public class ApplicationExitCommandContainer : Catel.MVVM.CommandContainerBase
{
    private readonly INavigationService _navigationService;

    public ApplicationExitCommandContainer(ICommandManager commandManager, INavigationService navigationService,
        IServiceProvider serviceProvider)
        : base(Commands.Application.Exit, commandManager, serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(navigationService);

        _navigationService = navigationService;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        await _navigationService.CloseApplicationAsync();
    }
}
