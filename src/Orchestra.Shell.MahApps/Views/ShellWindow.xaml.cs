namespace Orchestra.Views
{
    using System;
    using System.Windows.Data;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Windows;
    using MahApps.Metro.Controls;
    using MahApps.Metro.IconPacks;
    using Services;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class ShellWindow : IShell
    {
        private readonly IMahAppsService _mahAppsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellWindow"/> class.
        /// </summary>
        public ShellWindow()
            : base(DataWindowMode.Custom, setOwnerAndFocus: false)
        {
            var serviceLocator = ServiceLocator.Default;

            InitializeComponent();

            statusBar.Background = Orc.Theming.ThemeManager.Current.GetThemeColorBrush(Orc.Theming.ThemeColorStyle.AccentColor20);

            serviceLocator.RegisterInstance(pleaseWaitProgressBar, "busyIndicatorService");

            var statusService = serviceLocator.ResolveRequiredType<IStatusService>();
            statusService.Initialize(statusTextBlock);

            _mahAppsService = serviceLocator.ResolveRequiredType<IMahAppsService>();
            serviceLocator.RegisterInstance<IAboutInfoService>(_mahAppsService);

            var flyoutService = serviceLocator.ResolveRequiredType<IFlyoutService>();
            var flyouts = new FlyoutsControl();

            foreach (var flyout in flyoutService.GetFlyouts())
            {
                flyouts.Items.Add(flyout);
            }

            Flyouts = flyouts;
        }

        protected override async void OnLoaded(EventArgs e)
        {
            base.OnLoaded(e);

            var serviceLocator = ServiceLocator.Default;

            var commandManager = serviceLocator.ResolveRequiredType<ICommandManager>();

            var windowCommands = _mahAppsService.GetRightWindowCommands();

            var aboutInfo = await _mahAppsService.GetAboutInfoAsync();
            if (aboutInfo is not null)
            {
                var aboutWindowCommand = WindowCommandHelper.CreateWindowCommandButton(new PackIconMaterial { Kind = PackIconMaterialKind.Information }, "About");

                var aboutService = serviceLocator.ResolveRequiredType<IAboutService>();
#pragma warning disable AvoidAsyncVoid // Avoid async void
                commandManager.RegisterAction("Help.About", async () => await aboutService.ShowAboutAsync());
#pragma warning restore AvoidAsyncVoid // Avoid async void
                aboutWindowCommand.SetCurrentValue(System.Windows.Controls.Primitives.ButtonBase.CommandProperty, commandManager.GetCommand("Help.About"));

                windowCommands.Items.Add(aboutWindowCommand);
            }

            SetCurrentValue(RightWindowCommandsProperty, windowCommands);

            var statusBarContent = _mahAppsService.GetStatusBar();
            if (statusBarContent is not null)
            {
                customStatusBarItem.SetCurrentValue(ContentProperty, statusBarContent);
            }

            var mainView = _mahAppsService.GetMainView();
            if (mainView is null)
            {
                return;
            }

            contentControl.SetCurrentValue(ContentProperty, mainView);

            ShellDimensionsHelper.ApplyDimensions(this, mainView);

            SetBinding(TitleProperty, new Binding("ViewModel.Title")
            {
                Source = mainView
            });
        }
    }
}
