namespace Orchestra.Examples.Ribbon;

using System;
using Catel.MVVM;

public class ApplicationAboutCommandContainer : Catel.MVVM.CommandContainerBase
{
    private readonly IAboutService _aboutService;

    public ApplicationAboutCommandContainer(ICommandManager commandManager, 
        IServiceProvider serviceProvider, IAboutService aboutService)
        : base(Commands.Application.About, commandManager, serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(aboutService);

        _aboutService = aboutService;
    }

    public override void Execute(object parameter)
    {
        _aboutService.ShowAboutAsync();
    }
}
