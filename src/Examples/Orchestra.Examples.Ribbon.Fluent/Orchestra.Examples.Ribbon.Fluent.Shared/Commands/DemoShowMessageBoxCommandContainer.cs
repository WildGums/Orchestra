// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DemoShowMessageBoxCommandContainer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon
{
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

        protected override void Execute(object parameter)
        {
            var button = (MessageButton) parameter;

            switch (button)
            {
                case MessageButton.OK:
                    _messageService.Show("This is a custom message box implemented in Orchestra. Here is your long text", "Message box", button, MessageImage.Error);
                    break;
                case MessageButton.OKCancel:
                    _messageService.Show("This is a custom message box implemented in Orchestra. Here is your long text", "Message box", button, MessageImage.Information);
                    break;
                case MessageButton.YesNo:
                    _messageService.Show("This is a custom message box implemented in Orchestra. Here is your long text", "Message box", button, MessageImage.Warning);
                    break;
                case MessageButton.YesNoCancel:
                    _messageService.Show("This is a custom message box implemented in Orchestra. Here is your long text", "Message box", button, MessageImage.Warning);
                    break;
                default:
                    return;
            }
        }
    }
}