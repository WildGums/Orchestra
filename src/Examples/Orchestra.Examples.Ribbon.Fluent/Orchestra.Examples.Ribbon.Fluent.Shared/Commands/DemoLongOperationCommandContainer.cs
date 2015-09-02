// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DemoLongOperationCommandContainer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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
        #endregion

        #region Constructors
        public DemoLongOperationCommandContainer(ICommandManager commandManager,
            IPleaseWaitService pleaseWaitService)
            : base(Commands.Demo.LongOperation, commandManager)
        {
            _pleaseWaitService = pleaseWaitService;
        }
        #endregion

        protected override async Task ExecuteAsync(object parameter)
        {
            const int TotalItems = 250;

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