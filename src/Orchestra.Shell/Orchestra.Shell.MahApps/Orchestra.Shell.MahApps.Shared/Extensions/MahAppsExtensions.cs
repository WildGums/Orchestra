// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MahAppsExtensions.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Windows;
    using Catel;
    using Catel.Windows.Threading;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;

    public static class MahAppsExtensions
    {
        public static void Show(this BaseMetroDialog dialog)
        {
            Argument.IsNotNull(() => dialog);

            //dialog.ShowDialogExternally();

            var metroWindow = Application.Current.GetMainWindow();

            metroWindow.Dispatcher.Invoke(() => metroWindow.ShowMetroDialogAsync(dialog));
        }

        public static void ShowModal(this BaseMetroDialog dialog)
        {
            Argument.IsNotNull(() => dialog);

            dialog.ShowModalDialogExternally();
        }

        public static void Close(this BaseMetroDialog dialog, Window parentDialogWindow = null)
        {
            Argument.IsNotNull(() => dialog);

            if (parentDialogWindow != null)
            {
                parentDialogWindow.Close();
            }
            else
            {
                var mainWindow = Application.Current.GetMainWindow();
                mainWindow.HideMetroDialogAsync(dialog);
            }
        }

        public static MetroWindow GetMainWindow(this Application application)
        {
            Argument.IsNotNull(() => application);

            return (MetroWindow) application.MainWindow;
        }
    }
}