// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MahAppsUIVisualizerService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Catel.MVVM;
    using Catel.Services;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;

    public class MahAppsUIVisualizerService : UIVisualizerService
    {
        public MahAppsUIVisualizerService(IViewLocator viewLocator)
            : base(viewLocator)
        {
        }

        protected override Type WindowType
        {
            get { return typeof (Control); }
        }

        protected override bool? ShowWindow(FrameworkElement window, bool showModal)
        {
            var simpleDialog = window as CustomDialog;
            if (simpleDialog != null)
            {
                simpleDialog.Show();
                return true;
            }

            return base.ShowWindow(window, showModal);
        }

        protected override async Task<bool?> ShowWindowAsync(FrameworkElement window, bool showModal)
        {
            var simpleDialog = window as CustomDialog;
            if (simpleDialog != null)
            {
                if (showModal)
                {
                    var metroWindow = Application.Current.GetMainWindow();
                    await metroWindow.ShowMetroDialogAsync(simpleDialog);
                    await simpleDialog.WaitUntilUnloadedAsync();
                }
                else
                {
                    simpleDialog.Invoke(simpleDialog.Show);
                }

                return true;
            }

            return await base.ShowWindowAsync(window, showModal);
        }
    }
}