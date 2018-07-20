// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MahAppsMessageService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.Services;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;

    public class MahAppsMessageService : Catel.Services.MessageService
    {
        private readonly IDispatcherService _dispatcherService;
        private readonly ILanguageService _languageService;

        public MahAppsMessageService(IDispatcherService dispatcherService, ILanguageService languageService)
            : base(dispatcherService, languageService)
        {
            Argument.IsNotNull(() => languageService);

            _dispatcherService = dispatcherService;
            _languageService = languageService;
        }

        protected override Task<MessageResult> ShowMessageBoxAsync(string message, string caption = "", MessageButton button = MessageButton.OK, MessageImage icon = MessageImage.None)
        {
            var style = MessageDialogStyle.Affirmative;
            var affirmativeResult = MessageResult.OK;
            var negativeResult = MessageResult.No;
            var auxiliaryResult = MessageResult.Cancel;

            MetroDialogSettings settings = new MetroDialogSettings();

            switch (button)
            {
                case MessageButton.OK:
                    style = MessageDialogStyle.Affirmative;
                    affirmativeResult = MessageResult.OK;
                    settings.AffirmativeButtonText = _languageService.GetString("OK");
                    break;

                case MessageButton.OKCancel:
                    style = MessageDialogStyle.AffirmativeAndNegative;
                    affirmativeResult = MessageResult.OK;
                    negativeResult = MessageResult.Cancel;
                    settings.AffirmativeButtonText = _languageService.GetString("OK");
                    settings.NegativeButtonText = _languageService.GetString("Cancel");
                    break;

                case MessageButton.YesNo:
                    style = MessageDialogStyle.AffirmativeAndNegative;
                    affirmativeResult = MessageResult.Yes;
                    negativeResult = MessageResult.No;
                    settings.AffirmativeButtonText = _languageService.GetString("Yes");
                    settings.NegativeButtonText = _languageService.GetString("No");
                    break;

                case MessageButton.YesNoCancel:
                    style = MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary;
                    affirmativeResult = MessageResult.Yes;
                    negativeResult = MessageResult.No;
                    auxiliaryResult = MessageResult.Cancel;
                    settings.AffirmativeButtonText = _languageService.GetString("Yes");
                    settings.NegativeButtonText = _languageService.GetString("No");
                    settings.FirstAuxiliaryButtonText = _languageService.GetString("Cancel");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(button));
            }

            var tcs = new TaskCompletionSource<MessageResult>();

#pragma warning disable AvoidAsyncVoid
            _dispatcherService.BeginInvoke(async () =>
#pragma warning restore AvoidAsyncVoid
            {
                var window = Application.Current.MainWindow as MetroWindow;
                if (window == null)
                {
                    tcs.TrySetResult(MessageResult.Cancel);
                    return;
                }

                var result = await window.ShowMessageAsync(caption, message, style, settings);
                switch (result)
                {
                    case MessageDialogResult.Negative:
                        tcs.TrySetResult(negativeResult);
                        break;

                    case MessageDialogResult.Affirmative:
                        tcs.TrySetResult(affirmativeResult);
                        break;

                    case MessageDialogResult.FirstAuxiliary:
                        tcs.TrySetResult(auxiliaryResult);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(result));
                }
            });

            return tcs.Task;
        }
    }
}
