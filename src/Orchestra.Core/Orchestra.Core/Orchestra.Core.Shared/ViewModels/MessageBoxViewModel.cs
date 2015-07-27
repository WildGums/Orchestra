// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBoxViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System;
    using System.Text;
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

            OkCommand = new Command(OnOkCommandExecute);
            YesCommand = new Command(OnYesCommandExecute);
            NoCommand = new Command(OnNoCommandExecute);
            CancelCommand = new Command(OnCancelCommandExecute);
            EscapeCommand = new Command(OnEscapeCommandExecute);

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

        protected override async Task Close()
        {
            SetResult();

            await base.Close();
        }

        private void SetResult()
        {
            if (Result != MessageResult.None)
            {
                return;
            }

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

                default:
                    return;
            }
        }

        #region Commands
        public Command CopyToClipboard { get; private set; }

        private void OnCopyToClipboardExecute()
        {
            var text = _messageService.GetAsText(Message, Button);

            _clipboardService.CopyToClipboard(text);
        }

        public Command OkCommand { get; private set; }

        private async void OnOkCommandExecute()
        {
            Result = MessageResult.OK;
            await CloseViewModel(null);
        }

        public Command YesCommand { get; private set; }

        private async void OnYesCommandExecute()
        {
            Result = MessageResult.Yes;
            await CloseViewModel(null);
        }

        public Command NoCommand { get; private set; }

        private async void OnNoCommandExecute()
        {
            Result = MessageResult.No;
            await CloseViewModel(null);
        }

        public Command CancelCommand { get; private set; }

        private async void OnCancelCommandExecute()
        {
            Result = MessageResult.Cancel;
            await CloseViewModel(null);
        }

        public Command EscapeCommand { get; private set; }

        private void OnEscapeCommandExecute()
        {
            switch (Button)
            {
                case MessageButton.OK:
                    OnOkCommandExecute();
                    break;

                case MessageButton.OKCancel:
                    OnCancelCommandExecute();
                    break;

                case MessageButton.YesNo:
                    break;

                case MessageButton.YesNoCancel:
                    OnCancelCommandExecute();
                    break;

                default:
                    return;
            }
        }
        #endregion
    }
}