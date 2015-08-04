// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellWindow.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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
                AddCustomButton(new DataWindowButton("Keyboard shortcuts", () => uiVisualizerService.ShowDialog<KeyboardMappingsOverviewViewModel>()));
            }

            serviceLocator.RegisterInstance<IAboutInfoService>(taskRunnerService);

            if (taskRunnerService.GetAboutInfo() != null)
            {
                var aboutService = serviceLocator.ResolveType<IAboutService>();
                commandManager.RegisterAction("Help.About", aboutService.ShowAbout);

                AddCustomButton(new DataWindowButton("About", commandManager.GetCommand("Help.About")));
            }

            ThemeHelper.EnsureApplicationThemes(GetType().Assembly, true);

            InitializeComponent();
            serviceLocator.RegisterInstance<ILogControlService>(new LogControlService(traceOutputControl));

            ConfigurationContext = taskRunnerService.GetViewDataContext();

            var startupSize = taskRunnerService.GetInitialWindowSize();
            if (startupSize != null && !startupSize.IsEmpty)
            {
                bool setWidth = startupSize.Width > 0d;
                bool setHeight = startupSize.Height > 0d;

                if (setHeight && setWidth)
                {
                    SizeToContent = SizeToContent.Manual;
                }
                else if (setHeight)
                {
                    SizeToContent = SizeToContent.Width;
                }
                else if (setWidth)
                {
                    SizeToContent = SizeToContent.Height;
                }
                else
                {
                    SizeToContent = SizeToContent.WidthAndHeight;
                }

                if (setWidth)
                {
                    MinWidth = startupSize.Width;
                    Width = startupSize.Width;
                }

                if (setHeight)
                {
                    MinHeight = startupSize.Height;
                    Height = startupSize.Height;
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
            get { return (object)GetValue(ConfigurationContextProperty); }
            set { SetValue(ConfigurationContextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConfigurationContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConfigurationContextProperty =
            DependencyProperty.Register("ConfigurationContext", typeof(object), typeof(ShellWindow), new PropertyMetadata(null));
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

                view.DataContext = ConfigurationContext;
            }
        }
        #endregion
    }
}