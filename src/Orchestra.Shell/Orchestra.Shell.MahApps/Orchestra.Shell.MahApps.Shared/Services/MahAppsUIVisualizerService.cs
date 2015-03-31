// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MahAppsUIVisualizerService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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
    using Catel.Windows.Threading;
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

        protected override Task<bool?> ShowWindowAsync(FrameworkElement window, bool showModal)
        {
            var simpleDialog = window as CustomDialog;
            if (simpleDialog != null)
            {
                return Task<bool?>.Factory.StartNew(() =>
                {
                    simpleDialog.Dispatcher.Invoke(simpleDialog.Show);
                    return true;

                    //if (showModal)
                    //{
                    //    simpleDialog.Dispatcher.Invoke(simpleDialog.ShowModal);
                    //    return true;
                    //}
                    //else
                    //{
                    //    simpleDialog.Dispatcher.Invoke(simpleDialog.Show);
                    //    return true;
                    //}
                });
            }

            return base.ShowWindowAsync(window, showModal);
        }
    }
}