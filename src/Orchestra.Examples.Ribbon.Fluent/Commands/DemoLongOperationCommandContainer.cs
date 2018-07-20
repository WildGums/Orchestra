// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DemoLongOperationCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Threading;

    internal class DemoLongOperationCommandContainer : CommandContainerBase
    {
        #region Fields
        private readonly IPleaseWaitService _pleaseWaitService;
        private readonly IMessageService _messageService;
        #endregion

        #region Constructors
        public DemoLongOperationCommandContainer(ICommandManager commandManager, IPleaseWaitService pleaseWaitService, IMessageService messageService)
            : base(Commands.Demo.LongOperation, commandManager)
        {
            Argument.IsNotNull(() => pleaseWaitService);
            Argument.IsNotNull(() => messageService);

            _pleaseWaitService = pleaseWaitService;
            _messageService = messageService;
        }
        #endregion

        protected override async Task ExecuteAsync(object parameter)
        {
            const int TotalItems = 250;

            await _messageService.ShowAsync("App will now simulate a long-running processing with progress at the bottom");

            await TaskHelper.Run(async () =>
            {
                var random = new Random();

                for (var i = 0; i < TotalItems; i++)
                {
                    _pleaseWaitService.UpdateStatus(i + 1, TotalItems);

                    await TaskShim.Delay(random.Next(5, 30));
                }
            }, true);
        }
    }
}