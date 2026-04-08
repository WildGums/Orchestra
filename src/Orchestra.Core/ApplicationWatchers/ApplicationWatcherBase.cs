namespace Orchestra;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Catel.IoC;
using Catel.Logging;
using Catel.Services;
using Catel.Windows.Threading;
using Microsoft.Extensions.Logging;

public abstract class ApplicationWatcherBase : IConstructAtStartup
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(ApplicationWatcherBase));

    protected static IDispatcherService DispatcherService = default!;
    protected static IMainWindowService MainWindowService = default!;

    private static DispatcherTimerEx? DispatcherTimer;
    private static readonly Queue<Action<Window>> ShellActivatedActions;
    private static readonly object Lock = new object();

    static ApplicationWatcherBase()
    {
        ShellActivatedActions = new Queue<Action<Window>>();
    }

    protected ApplicationWatcherBase(IDispatcherService dispatcherService, IMainWindowService mainWindowService)
    {
        if (DispatcherService is null)
        {
            DispatcherService = dispatcherService;;
        }

        if (MainWindowService is null)
        {
            MainWindowService = mainWindowService;
        }

        if (DispatcherTimer is null)
        {
            DispatcherTimer = new DispatcherTimerEx(DispatcherService);

            // Hotfix: changed from 5 => 250, otherwise it will cause too much CPU usage
            DispatcherTimer.Interval = TimeSpan.FromMilliseconds(250);
            DispatcherTimer.Tick += async (sender, e) => await EnsureMainWindowAsync();
        }

        // Note: starting the timer here is useless since it's immediately stopped in EnsureMainWindowAsync
        //DispatcherTimer.Start();

        _ = EnsureMainWindowAsync();
    }

    protected void EnqueueShellActivatedAction(Action<Window> action)
    {
        lock (Lock)
        {
            ShellActivatedActions.Enqueue(action);
        }

        DispatcherTimer?.Start();
    }

    public static async Task EnsureMainWindowAsync()
    {
        DispatcherTimer?.Stop();

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
            DispatcherTimer?.Start();
            return;
        }

        if (ShellActivatedActions is null)
        {
            DispatcherTimer?.Start();
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
                    Logger.LogError(ex, "Failed to execute ApplicationWatcher action");
                }
            }
        }
    }

    private static bool NoShell()
    {
        return Application.Current is null;
    }
}
