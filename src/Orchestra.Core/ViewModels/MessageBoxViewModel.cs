namespace Orchestra.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Services;

    public class MessageBoxViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;
        private readonly IClipboardService _clipboardService;
        private readonly ILanguageService _languageService;

        public MessageBoxViewModel(IMessageService messageService, 
            IClipboardService clipboardService, ILanguageService languageService)
        {
            _messageService = messageService;
            _clipboardService = clipboardService;
            _languageService = languageService;

            ValidateUsingDataAnnotations = false;

            CopyToClipboard = new Command(OnCopyToClipboardExecute);

            OkCommand = new TaskCommand(OnOkCommandExecuteAsync);
            YesCommand = new TaskCommand(OnYesCommandExecuteAsync);
            NoCommand = new TaskCommand(OnNoCommandExecuteAsync);
            CancelCommand = new TaskCommand(OnCancelCommandExecuteAsync);
            EscapeCommand = new TaskCommand(OnEscapeCommandExecuteAsync);

            Result = MessageResult.None;

            OkText = _languageService.GetRequiredString("OK");
            YesText = _languageService.GetRequiredString("Yes");
            NoText = _languageService.GetRequiredString("No");
            CancelText = _languageService.GetRequiredString("Cancel");
        }

        public string? Message { get; set; }

        public string OkText { get; set; }

        public string YesText { get; set; }

        public string NoText { get; set; }

        public string CancelText { get; set; }

        public MessageResult Result { get; set; }

        public MessageButton Button { get; set; }

        public MessageImage Icon { get; set; }

        public void SetTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                Title = title;
                return;
            }

            var assembly = AssemblyHelper.GetRequiredEntryAssembly();
            Title = assembly.Title() ?? string.Empty;
        }

        protected override async Task CloseAsync()
        {
            if (Result == MessageResult.None)
            {
                switch (Button)
                {
                    case MessageButton.OK:
                        Result = MessageResult.OK;
                        break;

                    case MessageButton.OKCancel:
                        Result = MessageResult.Cancel;
                        break;

                    case MessageButton.YesNoCancel:
                        Result = MessageResult.Cancel;
                        break;
                }
            }

            await base.CloseAsync();
        }

        public Command CopyToClipboard { get; private set; }

        private void OnCopyToClipboardExecute()
        {
            var text = _messageService.GetAsText(Message ?? string.Empty, Button);

            _clipboardService.CopyToClipboard(text);
        }

        public TaskCommand OkCommand { get; private set; }

        private async Task OnOkCommandExecuteAsync()
        {
            Result = MessageResult.OK;
            await CloseViewModelAsync(null);
        }

        public TaskCommand YesCommand { get; private set; }

        private async Task OnYesCommandExecuteAsync()
        {
            Result = MessageResult.Yes;
            await CloseViewModelAsync(null);
        }

        public TaskCommand NoCommand { get; private set; }

        private async Task OnNoCommandExecuteAsync()
        {
            Result = MessageResult.No;
            await CloseViewModelAsync(null);
        }

        public TaskCommand CancelCommand { get; private set; }

        private async Task OnCancelCommandExecuteAsync()
        {
            Result = MessageResult.Cancel;
            await CloseViewModelAsync(null);
        }

        public TaskCommand EscapeCommand { get; private set; }

        private async Task OnEscapeCommandExecuteAsync()
        {
            switch (Button)
            {
                case MessageButton.OK:
                    await OnOkCommandExecuteAsync();
                    break;

                case MessageButton.OKCancel:
                    await OnCancelCommandExecuteAsync();
                    break;

                case MessageButton.YesNo:
                    break;

                case MessageButton.YesNoCancel:
                    await OnCancelCommandExecuteAsync();
                    break;
            }
        }
    }
}
