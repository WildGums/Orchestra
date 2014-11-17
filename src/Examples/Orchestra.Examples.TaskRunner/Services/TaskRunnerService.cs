// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskRunnerService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.TaskRunner.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel.Logging;
    using Models;
    using Orchestra.Models;
    using Orchestra.Services;
    using Views;

    public class TaskRunnerService : ITaskRunnerService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public string Title { get { return "Custom TaskRunner demo"; } }

        public bool ShowCustomizeShortcutsButton { get { return true; }}

        public async Task<object> GetViewDataContext()
        {
            return new Settings();
        }

        public FrameworkElement GetView()
        {
            return new SettingsView();
        }

        public async Task Run(object dataContext)
        {
            var settings = (Settings) dataContext;

            Log.Info("Running action with the following settings:");
            Log.Indent();
            Log.Info("Working directory => {0}", settings.WorkingDirectory);
            Log.Info("Output directory => {0}", settings.OutputDirectory);
            Log.Info("Current time => {0}", settings.CurrentTime);
            Log.Info("Horizon start => {0}", settings.HorizonStart);
            Log.Info("Horizon end => {0}", settings.HorizonEnd);
            Log.Unindent();

            Log.Info("Sleeping to show long running action with blocking thread");

            Thread.Sleep(2500);

            Log.Info("Action is complete!");
        }

        public Size GetInitialWindowSize()
        {
            return Size.Empty;
        }

        public AboutInfo GetAboutInfo()
        {
            return new AboutInfo();
        }
    }
}