namespace Orchestra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Orchestra.Services;

    public abstract class ApplicationWatcherBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        protected static readonly IDispatcherService DispatcherService;
        protected static readonly IMainWindowService MainWindowService;

        private static readonly DispatcherTimer DispatcherTimer;
        private static readonly Queue<Action<Window>> ShellActivatedActions;
        private static readonly object Lock = new object();

        static ApplicationWatcherBase()
        {
            ShellActivatedActions = new Queue<Action<Window>>();

            var serviceLocator = ServiceLocator.Default;
            DispatcherService = serviceLocator.ResolveRequiredType<IDispatcherService>();
            MainWindowService = serviceLocator.ResolveRequiredType<IMainWindowService>();

            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Interval = TimeSpan.FromMilliseconds(5);
            DispatcherTimer.Tick += async (sender, e) => await EnsureMainWindowAsync();
            DispatcherTimer.Start();

            _ = EnsureMainWindowAsync();
        }

        protected void EnqueueShellActivatedAction(Action<Window> action)
        {
            lock (Lock)
            {
                ShellActivatedActions.Enqueue(action);
            }

            DispatcherTimer.Start();
        }

        public static async Task EnsureMainWindowAsync()
        {
            DispatcherTimer.Stop();

            if (NoShell())
            {
                // Important for unit test compatibility
                return;
            }

            // Once the main window is visible, we can start running shell stuff, don't wait for the splash screen to be really hidden. So we
            // only exit if the splash screen is still the main window *or* the main window (shell) is not visible yet
            var mainWindow = await MainWindowService.GetMainWindowAsync();
            if (mainWindow is Orchestra.Views.SplashScreen || !(mainWindow?.IsVisible ?? false)) 
            {
                mainWindow = null;
            }

            if (mainWindow is null)
            {
                DispatcherTimer.Start();
                return;
            }

            if (ShellActivatedActions is null)
            {
                DispatcherTimer.Start();
                return;
            }

            lock (Lock)
            {
                while (ShellActivatedActions.Any())
                {
                    var action = ShellActivatedActions.Dequeue();

                    try
                    {
                        action(mainWindow);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Failed to execute ApplicationWatcher action");
                    }
                }
            }
        }

        private static bool NoShell()
        {
            return Application.Current is null;
        }
    }
}
