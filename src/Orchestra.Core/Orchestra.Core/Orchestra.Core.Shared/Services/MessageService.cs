// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBoxService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.Services;
    using Catel.Windows;
    using ViewModels;

    public class MessageService : Catel.Services.MessageService
    {
        private readonly IDispatcherService _dispatcherService;
        private readonly IUIVisualizerService _uiVisualizerService;

        public MessageService(IDispatcherService dispatcherService, IUIVisualizerService uiVisualizerService)
            : base(dispatcherService)
        {
            Argument.IsNotNull(() => dispatcherService);
            Argument.IsNotNull(() => uiVisualizerService);

            _dispatcherService = dispatcherService;
            _uiVisualizerService = uiVisualizerService;
        }

        public override Task<MessageResult> Show(string message, string caption = "", MessageButton button = MessageButton.OK, MessageImage icon = MessageImage.None)
        {

            Argument.IsNotNullOrWhitespace("message", message);

            var tcs = new TaskCompletionSource<MessageResult>();

            _dispatcherService.BeginInvoke(() =>
            {
                var viewModel = new MessageBoxViewModel
                {
                    Message = message,
                    Button = button,
                    Icon = icon
                };

                viewModel.SetTitle(caption);

                _dispatcherService.Invoke(() => _uiVisualizerService.ShowDialog(viewModel), true);

                tcs.SetResult(viewModel.Result);
            });

            return tcs.Task;

        }
    }
}