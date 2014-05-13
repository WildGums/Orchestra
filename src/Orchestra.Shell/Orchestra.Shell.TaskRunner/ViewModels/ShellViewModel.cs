// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System;
    using System.Threading;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Services;

    public class ShellViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ITaskRunnerService _taskRunnerService;

        // Note: will be replaced by BackgroundWorkerService in Catel
        private Thread _handlerThread;

        public ShellViewModel(ITaskRunnerService taskRunnerService)
        {
            Argument.IsNotNull("taskRunnerService", taskRunnerService);

            _taskRunnerService = taskRunnerService;

            Run = new Command(OnRunExecute, OnRunCanExecute);

            SuspendValidation = true;

            var assembly = AssemblyHelper.GetEntryAssembly();
            Title = assembly.Title();
        }

        #region Properties
        public bool IsRunning { get; private set; }

        public object ConfigurationContext { get; set; }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the Run command.
        /// </summary>
        public Command Run { get; private set; }

        private bool OnRunCanExecute()
        {
            if (IsRunning)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Method to invoke when the Run command is executed.
        /// </summary>
        private void OnRunExecute()
        {
            SuspendValidation = false;

            Validate(true);

            if (HasErrors)
            {
                Log.Warning("There are errors that need to be fixed, please do that before running.");
                foreach (var error in ValidationContext.GetErrors())
                {
                    Log.Warning("  * {0}", error.Message);
                }

                return;
            }

            IsRunning = true;

            _handlerThread = new Thread(() =>
            {
                try
                {
                    _taskRunnerService.Run(ConfigurationContext);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to execute the command");
                }
                finally
                {
                    IsRunning = false;

                    _handlerThread = null;
                }
            });

            _handlerThread.Start();
        }
        #endregion
    }
}