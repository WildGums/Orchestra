// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DemoHiddenCommandContainer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;

    public class DemoHiddenCommandContainer : CommandContainerBase
    {
        private readonly IMessageService _messageService;

        #region Constructors
        public DemoHiddenCommandContainer(ICommandManager commandManager, IMessageService messageService)
            : base(Commands.Demo.Hidden, commandManager)
        {
            Argument.IsNotNull(() => messageService);

            _messageService = messageService;
        }
        #endregion

        protected override async Task ExecuteAsync(object parameter)
        {
            await _messageService.ShowAsync("You just executed a hidden command");
        }
    }
}