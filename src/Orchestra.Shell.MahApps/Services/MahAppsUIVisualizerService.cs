namespace Orchestra.Services
{
    using System.Threading.Tasks;
    using System.Windows;
    using Windows;
    using Catel.MVVM;
    using Catel.Services;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;
    using System;

    public class MahAppsUIVisualizerService : UIVisualizerService
    {
        public MahAppsUIVisualizerService(IViewLocator viewLocator, IDispatcherService dispatcherService)
            : base(viewLocator, dispatcherService)
        {
        }

        public override async Task<UIVisualizerResult> ShowWindowAsync(FrameworkElement window, UIVisualizerContext context)
        {
            ArgumentNullException.ThrowIfNull(window);
            ArgumentNullException.ThrowIfNull(context);

            var simpleDialog = window as CustomDialog;
            if (simpleDialog is not null)
            {
                if (context.IsModal)
                {
                    var metroWindow = Application.Current.GetMainWindow();
                    await metroWindow.ShowMetroDialogAsync(simpleDialog);
                    await simpleDialog.WaitUntilUnloadedAsync();
                    var simpleDataWindow = window as SimpleDataWindow;
                    bool? result = true;
                    if (simpleDataWindow is not null)
                    {
                        result = simpleDataWindow.DialogResult;
                    }

                    return new UIVisualizerResult(result, context, window);
                }

                simpleDialog.Invoke(simpleDialog.Show);

                return new UIVisualizerResult(true, context, window);
            }

            var baseResult = await base.ShowWindowAsync(window, context);
            return baseResult;
        }
    }
}
