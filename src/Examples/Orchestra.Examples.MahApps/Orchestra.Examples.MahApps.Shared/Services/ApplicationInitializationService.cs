// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationInitializationService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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
            await InitializeCommands();

            await RunAndWaitAsync(new Func<Task>[]
            {
                InitializePerformance
            });

            Log.Debug("Creating flyouts");

            var dependencyResolver = this.GetDependencyResolver();
            var flyoutService = dependencyResolver.Resolve<IFlyoutService>();
            flyoutService.AddFlyout<PersonView>(ExampleEnvironment.PersonFlyoutName, Position.Right);
        }

        private async Task InitializeCommands()
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

        private async Task InitializePerformance()
        {
            Log.Info("Improving performance");

            Catel.Data.ModelBase.DefaultSuspendValidationValue = true;
            Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;
        }
    }
}