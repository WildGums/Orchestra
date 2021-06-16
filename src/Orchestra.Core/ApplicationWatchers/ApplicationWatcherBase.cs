namespace Orchestra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using ViewModels;

    public abstract class ApplicationWatcherBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        protected static readonly IDispatcherService DispatcherService;

        private static readonly DispatcherTimer DispatcherTimer;
        private static Queue<Action<Window>> ShellActivatedActions;
        private static readonly object Lock = new object();

        static ApplicationWatcherBase()
        {
            EnsureSubscribesInitialized();

            DispatcherService = ServiceLocator.Default.ResolveType<IDispatcherService>();

            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Interval = TimeSpan.FromMilliseconds(5);
            DispatcherTimer.Tick += (sender, e) => EnsureMainWindow();
            DispatcherTimer.Start();

            EnsureMainWindow();
        }

        protected void EnqueueShellActivatedAction(Action<Window> action)
        {
            EnsureSubscribesInitialized();

            lock (Lock)
            {
                ShellActivatedActions.Enqueue(action);
            }

            DispatcherTimer.Start();
        }

        private static void EnsureSubscribesInitialized()
        {
            if (ShellActivatedActions is null)
            {
                lock (Lock)
                {
                    ShellActivatedActions = new Queue<Action<Window>>();
                }
            }
        }

        private static void EnsureMainWindow()
        {
            DispatcherTimer.Stop();

            var mainWindow = System.Windows.Application.Current.MainWindow;
            if (mainWindow is Orchestra.Views.SplashScreen || SplashScreenViewModel.IsActive)
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
                        Log.Error(ex, LanguageHelper.GetString("Failed to execute ApplicationWatcher action"));
                    }
                }
            }
        }
    }
}
