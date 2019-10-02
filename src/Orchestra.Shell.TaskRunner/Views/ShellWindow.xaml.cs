// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellWindow.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
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

    /// <summary>
    /// Interaction logic for ShellWindow.xaml.
    /// </summary>
    public partial class ShellWindow : IShell
    {
        #region Fields
        private bool _hasUpdatedViewModel;
        #endregion

        #region Constructors
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

            var fileLogListener = new FileLogListener(currentLogFileName, 25*1000)
            {
                IgnoreCatelLogging = true,
                IsDebugEnabled = false
            };
            
            LogManager.AddListener(fileLogListener);

            var serviceLocator = this.GetServiceLocator();
            var taskRunnerService = serviceLocator.ResolveType<ITaskRunnerService>();
            var commandManager = serviceLocator.ResolveType<ICommandManager>();
            var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();

            if (taskRunnerService.ShowCustomizeShortcutsButton)
            {
                AddCustomButton(DataWindowButton.FromAsync("Keyboard shortcuts", () => uiVisualizerService.ShowDialogAsync<KeyboardMappingsOverviewViewModel>(), null));
            }

            serviceLocator.RegisterInstance<IAboutInfoService>(taskRunnerService);

            if (taskRunnerService.GetAboutInfo() != null)
            {
                var aboutService = serviceLocator.ResolveType<IAboutService>();
#pragma warning disable AvoidAsyncVoid // Avoid async void
                commandManager.RegisterAction("Help.About", async () => await aboutService.ShowAboutAsync());
#pragma warning restore AvoidAsyncVoid // Avoid async void

                AddCustomButton(new DataWindowButton("About", commandManager.GetCommand("Help.About")));
            }

            InitializeComponent();
            serviceLocator.RegisterInstance<ILogControlService>(new LogControlService(traceOutputControl));

            ConfigurationContext = taskRunnerService.GetViewDataContext();

            var startupSize = taskRunnerService.GetInitialWindowSize();
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

            var view = taskRunnerService.GetView();

            contentControl.Content = view;
        }
        #endregion

        #region Properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public object ConfigurationContext
        {
            get { return GetValue(ConfigurationContextProperty); }
            set { SetValue(ConfigurationContextProperty, value); }
        }

        public static readonly DependencyProperty ConfigurationContextProperty = DependencyProperty.Register(nameof(ConfigurationContext), typeof(object), 
            typeof(ShellWindow), new PropertyMetadata(null));
        #endregion

        #region Methods
        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            // Let the shell create its view model first. Then the view model of the dynamic view model will
            // be created (which means the parent / child view models will work).
            var view = contentControl.Content as FrameworkElement;
            if (view != null && !_hasUpdatedViewModel)
            {
                _hasUpdatedViewModel = true;

                view.SetValue(DataContextProperty, ConfigurationContext);
            }
        }
        #endregion
    }
}
