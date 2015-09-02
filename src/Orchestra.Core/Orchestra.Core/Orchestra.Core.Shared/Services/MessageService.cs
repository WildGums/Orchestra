// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using ViewModels;

    public class MessageService : Catel.Services.MessageService
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IDispatcherService _dispatcherService;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IViewModelFactory _viewModelFactory;
        #endregion

        #region Constructors
        public MessageService(IDispatcherService dispatcherService, IUIVisualizerService uiVisualizerService, IViewModelFactory viewModelFactory)
            : base(dispatcherService)
        {
            Argument.IsNotNull(() => dispatcherService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => viewModelFactory);

            _dispatcherService = dispatcherService;
            _uiVisualizerService = uiVisualizerService;
            _viewModelFactory = viewModelFactory;
        }
        #endregion

        public override Task<MessageResult> ShowAsync(string message, string caption = "", MessageButton button = MessageButton.OK, MessageImage icon = MessageImage.None)
        {
            Argument.IsNotNullOrWhitespace("message", message);

            Log.Info("Showing message to the user:\n\n{0}", this.GetAsText(message, button));

            var tcs = new TaskCompletionSource<MessageResult>();

            _dispatcherService.BeginInvoke(async () =>
            {
                var previousCursor = Mouse.OverrideCursor;
                Mouse.OverrideCursor = null;

                var vm = _viewModelFactory.CreateViewModel<MessageBoxViewModel>(null);

                vm.Message = message;
                vm.Button = button;
                vm.Icon = icon;

                vm.SetTitle(caption);

                await _uiVisualizerService.ShowDialogAsync(vm);

                Mouse.OverrideCursor = previousCursor;

                Log.Info("Result of message: {0}", vm.Result);

                tcs.SetResult(vm.Result);
            });

            return tcs.Task;
        }
    }
}