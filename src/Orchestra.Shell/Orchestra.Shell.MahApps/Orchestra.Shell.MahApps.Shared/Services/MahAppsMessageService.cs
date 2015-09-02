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
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;

    public class MahAppsMessageService : Catel.Services.MessageService
    {
        public MahAppsMessageService(IDispatcherService dispatcherService)
            : base(dispatcherService)
        {
        }

        protected override async Task<MessageResult> ShowMessageBoxAsync(string message, string caption = "", MessageButton button = MessageButton.OK, MessageImage icon = MessageImage.None)
        {
            var window = Application.Current.MainWindow as MetroWindow;
            if (window == null)
            {
                return MessageResult.Cancel;
            }

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

            var result = await window.ShowMessageAsync(caption, message, style);
            switch (result)
            {
                case MessageDialogResult.Negative:
                    return negativeResult;

                case MessageDialogResult.Affirmative:
                    return affirmativeResult;

                case MessageDialogResult.FirstAuxiliary:
                    return auxiliaryResult;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}