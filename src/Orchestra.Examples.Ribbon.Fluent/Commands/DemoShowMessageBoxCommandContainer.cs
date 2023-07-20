namespace Orchestra.Examples.Ribbon
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;

    internal class DemoShowMessageBoxCommandContainer : CommandContainerBase
    {
        private readonly IMessageService _messageService;

        public DemoShowMessageBoxCommandContainer(ICommandManager commandManager, IMessageService messageService)
            : base(Commands.Demo.ShowMessageBox, commandManager)
        {
            ArgumentNullException.ThrowIfNull(messageService);

            _messageService = messageService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (parameter is MessageButton)
            {
                var button = (MessageButton) parameter;

                switch (button)
                {
                    case MessageButton.OK:
                        await _messageService.ShowAsync("This is a custom message box implemented in Orchestra. Here is your long text. This is a custom message box implemented in Orchestra. Here is your long text. This is a custom message box implemented in Orchestra. Here is your long text. This is a custom message box implemented in Orchestra. Here is your long text. This is a custom message box implemented in Orchestra. Here is your long text. This is a custom message box implemented in Orchestra. Here is your long text. This is a custom message box implemented in Orchestra. Here is your long text.", null, button, MessageImage.Error);
                        break;

                    case MessageButton.OKCancel:
                        await _messageService.ShowAsync("This is a custom message box implemented in Orchestra. Here is your long text", null, button, MessageImage.Information);
                        break;

                    case MessageButton.YesNo:
                        await _messageService.ShowAsync("This is a custom message box implemented in Orchestra. Here is your long text", string.Empty, button, MessageImage.Warning);
                        break;

                    case MessageButton.YesNoCancel:
                        await _messageService.ShowAsync("This is a custom message box implemented in Orchestra. Here is your long text", string.Empty, button);
                        break;

                    default:
                        return;
                }
            }

            if (parameter is MessageImage)
            {
                var image = (MessageImage) parameter;

                await _messageService.ShowAsync($"This message should show the image '{image}'", icon: image);
            }
        }
    }
}
