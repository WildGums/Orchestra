// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MahAppsMessageService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel.Services;
    using Catel.Threading;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;

    public class MahAppsMessageService : Catel.Services.MessageService
    {
        private readonly IDispatcherService _dispatcherService;

        public MahAppsMessageService(IDispatcherService dispatcherService)
            : base(dispatcherService)
        {
            _dispatcherService = dispatcherService;
        }

        protected override Task<MessageResult> ShowMessageBoxAsync(string message, string caption = "", MessageButton button = MessageButton.OK, MessageImage icon = MessageImage.None)
        {
            var style = MessageDialogStyle.Affirmative;
            var affirmativeResult = MessageResult.OK;
            var negativeResult = MessageResult.No;
            var auxiliaryResult = MessageResult.Cancel;

            switch (button)
            {
                case MessageButton.OK:
                    style = MessageDialogStyle.Affirmative;
                    affirmativeResult = MessageResult.OK;
                    break;

                case MessageButton.OKCancel:
                    style = MessageDialogStyle.AffirmativeAndNegative;
                    affirmativeResult = MessageResult.OK;
                    negativeResult = MessageResult.Cancel;
                    break;

                case MessageButton.YesNo:
                    style = MessageDialogStyle.AffirmativeAndNegative;
                    affirmativeResult = MessageResult.Yes;
                    negativeResult = MessageResult.No;
                    break;

                case MessageButton.YesNoCancel:
                    style = MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary;
                    affirmativeResult = MessageResult.Yes;
                    negativeResult = MessageResult.No;
                    auxiliaryResult = MessageResult.Cancel;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("button");
            }

            var tcs = new TaskCompletionSource<MessageResult>();

            _dispatcherService.BeginInvoke(async () =>
            {
                var window = Application.Current.MainWindow as MetroWindow;
                if (window == null)
                {
                    tcs.SetResult(MessageResult.Cancel);
                    return;
                }

                var result = await window.ShowMessageAsync(caption, message, style);
                switch (result)
                {
                    case MessageDialogResult.Negative:
                        tcs.SetResult(negativeResult);
                        break;

                    case MessageDialogResult.Affirmative:
                        tcs.SetResult(affirmativeResult);
                        break;

                    case MessageDialogResult.FirstAuxiliary:
                        tcs.SetResult(auxiliaryResult);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });

            return tcs.Task;
        }
    }
}