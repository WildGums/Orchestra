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
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IDispatcherService _dispatcherService;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ILanguageService _languageService;
        private readonly IClipboardService _clipboardService;

        public MessageService(IDispatcherService dispatcherService, IUIVisualizerService uiVisualizerService,
            IViewModelFactory viewModelFactory, ILanguageService languageService, IClipboardService clipboardService)
            : base(dispatcherService, languageService)
        {
            _dispatcherService = dispatcherService;
            _uiVisualizerService = uiVisualizerService;
            _viewModelFactory = viewModelFactory;
            _languageService = languageService;
            _clipboardService = clipboardService;
        }

        public override Task<MessageResult> ShowAsync(string message, string caption = "", MessageButton button = MessageButton.OK, MessageImage icon = MessageImage.None)
        {
            Argument.IsNotNullOrWhitespace("message", message);

            Log.Info("Showing message to the user:\n\n{0}", this.GetAsText(message, button));

            var tcs = new TaskCompletionSource<MessageResult>();

            _dispatcherService.BeginInvokeIfRequired(async () =>
            {
                var vm = new MessageBoxViewModel(this, _clipboardService, _languageService);

                using (new DisposableToken<CursorMemory>(new CursorMemory(),
                    x =>
                    {
                        x.Instance.PreviousCursor = Mouse.OverrideCursor;
                        Mouse.OverrideCursor = null;
                    },
                    x =>
                    {
                        Mouse.OverrideCursor = x.Instance.PreviousCursor;
                    }))
                {

                    vm.Message = message;
                    vm.Button = button;
                    vm.Icon = icon;

                    vm.SetTitle(caption);

                    await _uiVisualizerService.ShowDialogAsync(vm);
                }

                Log.Info("Result of message: {0}", vm.Result);

                tcs.TrySetResult(vm.Result);
            });

            return tcs.Task;
        }

        private class CursorMemory
        {
            public Cursor? PreviousCursor { get; set; }
        }
    }
}
