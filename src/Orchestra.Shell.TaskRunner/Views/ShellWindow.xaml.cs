﻿namespace Orchestra.Views
{
    using System.IO;
    using System.Windows;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.MVVM.Views;
    using Catel.Services;
    using Catel.Windows;
    using ViewModels;

    using Services;
    using System;

    /// <summary>
    /// Interaction logic for ShellWindow.xaml.
    /// </summary>
    public partial class ShellWindow : IShell
    {
        private readonly ITaskRunnerService _taskRunnerService;

        private bool _hasUpdatedViewModel;

        static ShellWindow()
        {
            typeof(ShellWindow).AutoDetectViewPropertiesToSubscribe();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellWindow"/> class.
        /// </summary>
        /// <remarks>This method is required for design time support.</remarks>
        public ShellWindow()
            : base(DataWindowMode.Custom, setOwnerAndFocus: false)
        {
            var currentLogFileName = TaskRunnerEnvironment.CurrentLogFileName;
            if (File.Exists(currentLogFileName))
            {
                File.Delete(currentLogFileName);
            }

            var fileLogListener = new FileLogListener(currentLogFileName, 25 * 1000)
            {
                IgnoreCatelLogging = true,
                IsDebugEnabled = false
            };

            LogManager.AddListener(fileLogListener);

#pragma warning disable IDISP001 // Dispose created.
            var serviceLocator = this.GetServiceLocator();
#pragma warning restore IDISP001 // Dispose created.

            var uiVisualizerService = serviceLocator.ResolveRequiredType<IUIVisualizerService>();
            _taskRunnerService = serviceLocator.ResolveRequiredType<ITaskRunnerService>();

            if (_taskRunnerService.ShowCustomizeShortcutsButton)
            {
                AddCustomButton(DataWindowButton.FromAsync("Keyboard shortcuts", () => uiVisualizerService.ShowDialogAsync<KeyboardMappingsOverviewViewModel>(), null));
            }

            serviceLocator.RegisterInstance<IAboutInfoService>(_taskRunnerService);

            var commandManager = serviceLocator.ResolveRequiredType<ICommandManager>();

            var helpAboutCommand = commandManager.GetCommand("Help.About");
            if (helpAboutCommand is not null)
            {
                var aboutService = serviceLocator.ResolveRequiredType<IAboutService>();
                commandManager.RegisterAction("Help.About", async () => await aboutService.ShowAboutAsync());

                AddCustomButton(new DataWindowButton("About", helpAboutCommand));
            }

            InitializeComponent();

            serviceLocator.RegisterInstance<ILogControlService>(new LogControlService(traceOutputControl));

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
