// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Orchestra.Services;
    using Orchestra.Views;
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
#if DEBUG
            LogManager.AddDebugListener(true);
#endif

            var serviceLocator = ServiceLocator.Default;
            var shellService = serviceLocator.ResolveType<IShellService>();
            shellService.CreateWithSplash<ShellWindow>(PreInitialize, InitializeCommands, PostInitialize);

            _end = DateTime.Now;
            _stopwatch.Stop();

            Log.Info("Elapsed startup stopwatch time: {0}", _stopwatch.Elapsed);
            Log.Info("Elapsed startup time: {0}", _end - _start);
        }

        private void PreInitialize()
        {
            Log.Info("Improving performance");

            Catel.Data.ModelBase.DefaultSuspendValidationValue = true;
            Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;
        }

        private void InitializeCommands(ICommandManager commandManager)
        {
            commandManager.CreateCommand("File.Open", new InputGesture(Key.O, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);
            commandManager.CreateCommand("File.SaveToImage", new InputGesture(Key.I, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);
            commandManager.CreateCommand("File.Print", new InputGesture(Key.P, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);
            commandManager.CreateCommand("File.Exit", throwExceptionWhenCommandIsAlreadyCreated: false);

            commandManager.CreateCommand("Help.About", throwExceptionWhenCommandIsAlreadyCreated: false);
        }

        private void PostInitialize()
        {
            Log.Info("Delay to show the splash screen");

            Thread.Sleep(2500);
        }
        #endregion
    }
}