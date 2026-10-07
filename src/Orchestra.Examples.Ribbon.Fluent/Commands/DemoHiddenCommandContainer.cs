namespace Orchestra.Examples.Ribbon;

using System;
using System.Threading.Tasks;
using Catel.MVVM;
using Catel.Services;

public class DemoHiddenCommandContainer : CommandContainerBase
{
    private readonly IMessageService _messageService;
    private readonly ILanguageService _languageService;

    public DemoHiddenCommandContainer(ICommandManager commandManager, IMessageService messageService,
        ILanguageService languageService, IServiceProvider serviceProvider)
        : base(Commands.Demo.Hidden, commandManager, serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(messageService);
        ArgumentNullException.ThrowIfNull(languageService);

        _messageService = messageService;
        _languageService = languageService;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        await _messageService.ShowAsync(_languageService.GetRequiredString("Orchestra_Examples_Ribbon_DemoHidden_ExecutedMessage"));
    }
}
