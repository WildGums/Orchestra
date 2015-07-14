// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Services;
    using ViewModels;

    public class MessageService : Catel.Services.MessageService
    {
        #region Fields
        private readonly IDispatcherService _dispatcherService;
        private readonly IUIVisualizerService _uiVisualizerService;
        #endregion

        #region Constructors
        public MessageService(IDispatcherService dispatcherService, IUIVisualizerService uiVisualizerService)
            : base(dispatcherService)
        {
            Argument.IsNotNull(() => dispatcherService);
            Argument.IsNotNull(() => uiVisualizerService);

            _dispatcherService = dispatcherService;
            _uiVisualizerService = uiVisualizerService;
        }
        #endregion

        public override Task<MessageResult> Show(string message, string caption = "", MessageButton button = MessageButton.OK, MessageImage icon = MessageImage.None)
        {
            Argument.IsNotNullOrWhitespace("message", message);

            var tcs = new TaskCompletionSource<MessageResult>();

            _dispatcherService.BeginInvoke(async () =>
            {
                var viewModel = new MessageBoxViewModel
                {
                    Message = message,
                    Button = button,
                    Icon = icon
                };

                viewModel.SetTitle(caption);

                await _uiVisualizerService.ShowDialogAsync(viewModel);

                tcs.SetResult(viewModel.Result);
            });

            return tcs.Task;
        }
    }
}