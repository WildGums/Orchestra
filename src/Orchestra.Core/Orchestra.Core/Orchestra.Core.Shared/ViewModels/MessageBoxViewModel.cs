// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBoxViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;

    public class MessageBoxViewModel : ViewModelBase
    {
        #region Constructors
        public MessageBoxViewModel()
        {
            OkCommand = new Command(OnOkCommandExecute);
            YesCommand = new Command(OnYesCommandExecute);
            NoCommand = new Command(OnNoCommandExecute);
            CancelCommand = new Command(OnCancelCommandExecute);
            EscapeCommand = new Command(OnEscapeCommandExecute);
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

        #region Commands
        public Command OkCommand { get; private set; }

        private async void OnOkCommandExecute()
        {
            Result= MessageResult.OK;
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

        public Command EscapeCommand { get; set; }

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
                    OnNoCommandExecute();
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