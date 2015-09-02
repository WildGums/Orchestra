// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DemoShowMessageBoxCommandContainer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;

    internal class DemoShowMessageBoxCommandContainer : CommandContainerBase
    {
        #region Fields
        private readonly IMessageService _messageService;
        #endregion

        #region Constructors
        public DemoShowMessageBoxCommandContainer(ICommandManager commandManager, IMessageService messageService)
            : base(Commands.Demo.ShowMessageBox, commandManager)
        {
            Argument.IsNotNull(() => messageService);

            _messageService = messageService;
        }
        #endregion

        protected override async Task ExecuteAsync(object parameter)
        {
            var button = (MessageButton)parameter;

            switch (button)
            {
                case MessageButton.OK:
                    await _messageService.ShowAsync("This is a custom message box implemented in Orchestra. Here is your long text. This is a custom message box implemented in Orchestra. Here is your long text. This is a custom message box implemented in Orchestra. Here is your long text. This is a custom message box implemented in Orchestra. Here is your long text. This is a custom message box implemented in Orchestra. Here is your long text. This is a custom message box implemented in Orchestra. Here is your long text. This is a custom message box implemented in Orchestra. Here is your long text.", null, button, MessageImage.Error);
                    break;

                case MessageButton.OKCancel:
                    await _messageService.ShowAsync("This is a custom message box implemented in Orchestra. Here is your long text", null, button, MessageImage.Information);
                    break;

                case MessageButton.YesNo:
                    await _messageService.ShowAsync("This is a custom message box implemented in Orchestra. Here is your long text", "", button, MessageImage.Warning);
                    break;

                case MessageButton.YesNoCancel:
                    await _messageService.ShowAsync("This is a custom message box implemented in Orchestra. Here is your long text", "", button);
                    break;

                default:
                    return;
            }
        }
    }
}