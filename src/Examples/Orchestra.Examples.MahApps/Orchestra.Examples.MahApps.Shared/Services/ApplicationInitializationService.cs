// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationInitializationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.MahApps.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using global::MahApps.Metro.Controls;
    using Orchestra.Services;
    using Views;
    using InputGesture = Catel.Windows.Input.InputGesture;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        public override async Task InitializeBeforeCreatingShellAsync()
        {
            // Non-async first
            await InitializeCommandsAsync();

            await RunAndWaitAsync(new Func<Task>[]
            {
                InitializePerformanceAsync
            });

            Log.Debug("Creating flyouts");

            var dependencyResolver = this.GetDependencyResolver();
            var flyoutService = dependencyResolver.Resolve<IFlyoutService>();
            flyoutService.AddFlyout<PersonView>(ExampleEnvironment.PersonFlyoutName, Position.Right, flyoutTheme: FlyoutTheme.Accent);
        }

        private async Task InitializeCommandsAsync()
        {
            var commandManager = ServiceLocator.Default.ResolveType<ICommandManager>();

            commandManager.CreateCommand("File.Refresh", new InputGesture(Key.R, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);
            commandManager.CreateCommand("File.Save", new InputGesture(Key.S, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);
            commandManager.CreateCommand("File.Exit", throwExceptionWhenCommandIsAlreadyCreated: false);

            commandManager.CreateCommand("Help.About", throwExceptionWhenCommandIsAlreadyCreated: false);
        }

        public override async Task InitializeAfterCreatingShellAsync()
        {
            Log.Info("Delay to show the splash screen");

            //Thread.Sleep(2500);
        }

        private async Task InitializePerformanceAsync()
        {
            Log.Info("Improving performance");

            Catel.Data.ModelBase.DefaultSuspendValidationValue = true;
            Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;
        }
    }
}