namespace Orchestra.Views
{
    using System;
    using System.Windows;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.MVVM.Views;
    using Catel.Services;
    using Catel.Windows;
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using ViewModels;

    /// <summary>
    /// Interaction logic for ShellWindow.xaml.
    /// </summary>
    public partial class ShellWindow : IShell
    {
        private readonly ITaskRunnerService _taskRunnerService;

        private bool _hasUpdatedViewModel;

        public ShellWindow(IServiceProvider serviceProvider, IWrapControlService wrapControlService, 
            ILanguageService languageService, IUIVisualizerService uiVisualizerService,
            ITaskRunnerService taskRunnerService, ICommandManager commandManager,
            IAboutService aboutService)
            : base(serviceProvider, wrapControlService, languageService)
        {
            Mode = DataWindowMode.Custom;

            _taskRunnerService = taskRunnerService;
            if (_taskRunnerService.ShowCustomizeShortcutsButton)
            {
                AddCustomButton(DataWindowButton.FromAsync(serviceProvider, "Keyboard shortcuts", () => uiVisualizerService.ShowDialogAsync<KeyboardMappingsOverviewViewModel>(), null));
            }

            var helpAboutCommand = commandManager.GetCommand("Help.About");
            if (helpAboutCommand is not null)
            {
                commandManager.RegisterAction("Help.About", async () => await aboutService.ShowAboutAsync());

                AddCustomButton(new DataWindowButton("About", helpAboutCommand));
            }

            InitializeComponent();

            ConfigurationContext = _taskRunnerService.GetViewDataContext();

            var startupSize = _taskRunnerService.GetInitialWindowSize();
            if (!startupSize.IsEmpty)
            {
                var setWidth = startupSize.Width > 0d;
                var setHeight = startupSize.Height > 0d;

                if (setHeight && setWidth)
                {
                    SetCurrentValue(SizeToContentProperty, SizeToContent.Manual);
                }
                else if (setHeight)
                {
                    SetCurrentValue(SizeToContentProperty, SizeToContent.Width);
                }
                else if (setWidth)
                {
                    SetCurrentValue(SizeToContentProperty, SizeToContent.Height);
                }
                else
                {
                    SetCurrentValue(SizeToContentProperty, SizeToContent.WidthAndHeight);
                }

                if (setWidth)
                {
                    SetCurrentValue(MinWidthProperty, startupSize.Width);
                    SetCurrentValue(WidthProperty, startupSize.Width);
                }

                if (setHeight)
                {
                    SetCurrentValue(MinHeightProperty, startupSize.Height);
                    SetCurrentValue(HeightProperty, startupSize.Height);
                }
            }

            var view = _taskRunnerService.GetView();

            contentControl.Content = view;
        }

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public object? ConfigurationContext
        {
            get { return GetValue(ConfigurationContextProperty); }
            set { SetValue(ConfigurationContextProperty, value); }
        }

        public static readonly DependencyProperty ConfigurationContextProperty = DependencyProperty.Register(nameof(ConfigurationContext), typeof(object),
            typeof(ShellWindow), new PropertyMetadata(null));

        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            // Let the shell create its view model first. Then the view model of the dynamic view model will
            // be created (which means the parent / child view models will work).
            var view = contentControl.Content as FrameworkElement;
            if (view is not null && !_hasUpdatedViewModel)
            {
                _hasUpdatedViewModel = true;

                view.SetValue(DataContextProperty, ConfigurationContext);
            }
        }
    }
}
