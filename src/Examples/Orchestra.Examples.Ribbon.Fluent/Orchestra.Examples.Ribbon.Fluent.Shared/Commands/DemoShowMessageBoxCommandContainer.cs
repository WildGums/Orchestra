// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DemoShowMessageBoxCommandContainer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon
{
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Orchestra.Services;
    using Orchestra.ViewModels;

    internal class DemoShowMessageBoxCommandContainer : CommandContainerBase
    {
        private readonly IMessageBoxService _messageBoxService;

        public DemoShowMessageBoxCommandContainer(ICommandManager commandManager, IMessageBoxService messageBoxService)
            : base(Commands.Demo.ShowMessageBox, commandManager)
        {
            Argument.IsNotNull(() => messageBoxService);

            _messageBoxService = messageBoxService;
        }

        protected override void Execute(object parameter)
        {
            _messageBoxService.Show("This is a custom message box", "Message box", MessageButton.OKCancel);
        }
    }
}