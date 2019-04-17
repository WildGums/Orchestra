// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellWindow.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using Catel.IoC;
    using Catel.Windows;
    using Services;

    /// <summary>
    /// Interaction logic for ShellWindow.xaml.
    /// </summary>
    public partial class ShellWindow : IShell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellWindow"/> class.
        /// </summary>
        public ShellWindow()
        {
            var serviceLocator = ServiceLocator.Default;

            InitializeComponent();

            serviceLocator.RegisterInstance(pleaseWaitProgressBar, "pleaseWaitService");

            var statusService = serviceLocator.ResolveType<IStatusService>();
            statusService.Initialize(statusTextBlock);

            var dependencyResolver = this.GetDependencyResolver();
            var ribbonService = dependencyResolver.Resolve<IRibbonService>();

            var ribbonContent = ribbonService.GetRibbon();
            if (ribbonContent != null)
            {
                ribbonContentControl.SetCurrentValue(ContentProperty, ribbonContent);

                var ribbon = ribbonContent.FindVisualDescendantByType<Fluent.Ribbon>();
                if (ribbon != null)
                {
                    serviceLocator.RegisterInstance<Fluent.Ribbon>(ribbon);
                }
            }

            var statusBarContent = ribbonService.GetStatusBar();
            if (statusBarContent != null)
            {
                customStatusBarItem.SetCurrentValue(ContentProperty, statusBarContent);
            }

            var mainView = ribbonService.GetMainView();
            contentControl.Content = mainView;

            ShellDimensionsHelper.ApplyDimensions(this, mainView);
        }
    }
}
