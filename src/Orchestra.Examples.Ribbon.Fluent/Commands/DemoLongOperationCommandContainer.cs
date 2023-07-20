namespace Orchestra.Examples.Ribbon
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;

    internal class DemoLongOperationCommandContainer : CommandContainerBase
    {
        private readonly IBusyIndicatorService _busyIndicatorService;
        private readonly IMessageService _messageService;

        public DemoLongOperationCommandContainer(ICommandManager commandManager, IBusyIndicatorService busyIndicatorService, IMessageService messageService)
            : base(Commands.Demo.LongOperation, commandManager)
        {
            ArgumentNullException.ThrowIfNull(busyIndicatorService);
            ArgumentNullException.ThrowIfNull(messageService);

            _busyIndicatorService = busyIndicatorService;
            _messageService = messageService;
        }
  
        public override async Task ExecuteAsync(object parameter)
        {
            const int TotalItems = 250;

            await _messageService.ShowAsync("App will now simulate a long-running processing with progress at the bottom");

            await Task.Run(async () =>
            {
                var random = new Random();

                for (var i = 0; i < TotalItems; i++)
                {
                    _busyIndicatorService.UpdateStatus(i + 1, TotalItems);

                    await Task.Delay(random.Next(5, 30));
                }
            });
        }
    }
}
