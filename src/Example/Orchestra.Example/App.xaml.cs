// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Example
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Services;
    using Views;
    using InputGesture = Catel.Windows.Input.InputGesture;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly DateTime _start;
        private readonly Stopwatch _stopwatch;
        private DateTime _end;
        #endregion

        #region Constructors
        public App()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            _start = DateTime.Now;
        }
        #endregion

        #region Methods
        protected override void OnStartup(StartupEventArgs e)
        {
            var splashScreen = new Orchestra.Views.SplashScreen();
            splashScreen.Background = Brushes.Coral;
            splashScreen.Show();

#if DEBUG
            LogManager.AddDebugListener(true);
#endif

            Log.Info("Improving performance");

            Catel.Data.ModelBase.DefaultSuspendValidationValue = true;
            Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;

            var serviceLocator = ServiceLocator.Default;

            Log.Info("Calling base.OnStartup");

            base.OnStartup(e);

            Log.Info("Initializing commands");

            var commandManager = InitializeCommands();

            Log.Info("Loading keyboard mappings");

            var keyboardMappingsService = serviceLocator.ResolveType<IKeyboardMappingsService>();
            keyboardMappingsService.Load();

            _end = DateTime.Now;
            _stopwatch.Stop();

            Log.Info("Delay to show the splash screen");

            Thread.Sleep(2500);

            Log.Info("Elapsed startup stopwatch time: {0}", _stopwatch.Elapsed);
            Log.Info("Elapsed startup time: {0}", _end - _start);

            MainWindow = new MainWindow();

            // Now we have a new window, resubscribe the command manager
            commandManager.SubscribeToKeyboardEvents();

            MainWindow.Show();

            splashScreen.Close();
        }

        private ICommandManager InitializeCommands()
        {
            var dependencyResolver = this.GetDependencyResolver();
            var commandManager = dependencyResolver.Resolve<ICommandManager>();

            commandManager.CreateCommand("File.Open", new InputGesture(Key.O, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);
            commandManager.CreateCommand("File.SaveToImage", new InputGesture(Key.I, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);
            commandManager.CreateCommand("File.Print", new InputGesture(Key.P, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);
            commandManager.CreateCommand("File.Exit", throwExceptionWhenCommandIsAlreadyCreated: false);

            commandManager.CreateCommand("Help.About", throwExceptionWhenCommandIsAlreadyCreated: false);

            return commandManager;
        }
        #endregion
    }
}