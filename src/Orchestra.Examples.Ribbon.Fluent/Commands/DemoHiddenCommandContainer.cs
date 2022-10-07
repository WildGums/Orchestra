namespace Orchestra.Examples.Ribbon
{
    using System;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Services;

    public class DemoHiddenCommandContainer : CommandContainerBase
    {
        private readonly IMessageService _messageService;

        public DemoHiddenCommandContainer(ICommandManager commandManager, IMessageService messageService)
            : base(Commands.Demo.Hidden, commandManager)
        {
            ArgumentNullException.ThrowIfNull(messageService);

            _messageService = messageService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            await _messageService.ShowAsync("You just executed a hidden command");
        }
    }
}
