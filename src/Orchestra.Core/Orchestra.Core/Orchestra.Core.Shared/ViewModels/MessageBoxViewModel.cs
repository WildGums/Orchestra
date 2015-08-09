// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBoxViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Services;

    public class MessageBoxViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;
        private readonly IClipboardService _clipboardService;

        #region Constructors
        public MessageBoxViewModel(IMessageService messageService, IClipboardService clipboardService)
        {
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => clipboardService);

            _messageService = messageService;
            _clipboardService = clipboardService;

            CopyToClipboard = new Command(OnCopyToClipboardExecute);

            OkCommand = new TaskCommand(OnOkCommandExecuteAsync);
            YesCommand = new TaskCommand(OnYesCommandExecuteAsync);
            NoCommand = new TaskCommand(OnNoCommandExecuteAsync);
            CancelCommand = new TaskCommand(OnCancelCommandExecuteAsync);
            EscapeCommand = new TaskCommand(OnEscapeCommandExecuteAsync);

            Result = MessageResult.None;
        }
        #endregion

        #region Properties
        public string Message { get; set; }

        public MessageResult Result { get; set; }

        public MessageButton Button { get; set; }

        public MessageImage Icon { get; set; }
        #endregion

        public void SetTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                Title = title;
                return;
            }

            var assembly = AssemblyHelper.GetEntryAssembly();
            Title = assembly.Title();
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

        #region Commands
        public Command CopyToClipboard { get; private set; }

        private void OnCopyToClipboardExecute()
        {
            var text = _messageService.GetAsText(Message, Button);

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
        #endregion
    }
}