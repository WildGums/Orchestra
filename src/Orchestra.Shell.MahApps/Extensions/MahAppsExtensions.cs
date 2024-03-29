﻿namespace Orchestra
{
    using System;
    using System.Windows;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;

    public static class MahAppsExtensions
    {
        public static void Show(this BaseMetroDialog dialog)
        {
            ArgumentNullException.ThrowIfNull(dialog);

            var metroWindow = Application.Current.GetMainWindow();

            metroWindow.Dispatcher.Invoke(() => metroWindow.ShowMetroDialogAsync(dialog));
        }

        public static void ShowModal(this BaseMetroDialog dialog)
        {
            ArgumentNullException.ThrowIfNull(dialog);

            dialog.ShowModalDialogExternally();
        }

        public static void Close(this BaseMetroDialog dialog, Window? parentDialogWindow = null)
        {
            ArgumentNullException.ThrowIfNull(dialog);

            if (parentDialogWindow is not null)
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
            ArgumentNullException.ThrowIfNull(application);

            return (MetroWindow) application.MainWindow;
        }
    }
}
