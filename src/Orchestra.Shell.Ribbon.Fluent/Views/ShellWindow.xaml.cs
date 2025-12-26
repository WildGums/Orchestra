namespace Orchestra.Views
{
    using System;
    using Catel.Windows;
    using Microsoft.Extensions.DependencyInjection;

    public partial class ShellWindow : IShell
    {
        public ShellWindow(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            InitializeComponent();

            var statusService = serviceProvider.GetRequiredService<IStatusService>();
            statusService.Initialize(statusTextBlock);

            var ribbonService = serviceProvider.GetRequiredService<IRibbonService>();

            var ribbonContent = ribbonService.GetRibbon();
            if (ribbonContent is not null)
            {
                ribbonContentControl.SetCurrentValue(ContentProperty, ribbonContent);

                var ribbon = ribbonContent.FindVisualDescendantByType<Fluent.Ribbon>();
                if (ribbon is not null)
                {
                    //serviceLocator.RegisterInstance<Fluent.Ribbon>(ribbon);
                }
            }

            var statusBarContent = ribbonService.GetStatusBar();
            if (statusBarContent is not null)
            {
                customStatusBarItem.SetCurrentValue(ContentProperty, statusBarContent);
            }

            var mainView = ribbonService.GetMainView();
            if (mainView is not null)
            {
                contentControl.Content = mainView;

                ShellDimensionsHelper.ApplyDimensions(this, mainView);
            }
        }
    }
}
