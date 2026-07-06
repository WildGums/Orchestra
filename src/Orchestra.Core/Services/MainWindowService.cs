namespace Orchestra;

using System;
using System.Threading.Tasks;
using System.Windows;

public class MainWindowService : IMainWindowService
{
    private Window? _lastKnownMainWindow;

    public MainWindowService()
    {
        var application = Application.Current;
        if (application is null)
        {
            // Possible during unit tests
            return;
        }

        _lastKnownMainWindow = application.MainWindow;

        // Note: don't enable yet, we need to carefully test performance first
        //EventManager.RegisterClassHandler(typeof(Window), FrameworkElement.SizeChangedEvent, new RoutedEventHandler(OnSizeChanged));
    }

    public event EventHandler<EventArgs>? MainWindowChanged;

    public virtual async Task<Window?> GetMainWindowAsync()
    {
        var mainWindow = Application.Current.MainWindow; 
        if (mainWindow is Views.SplashScreen)
        {
            return null;
        }

        return mainWindow;
    }

    private async void OnSizeChanged(object? sender, RoutedEventArgs e)
    {
        await CheckForUpdatedMainWindowAsync();
    }

    protected virtual async Task CheckForUpdatedMainWindowAsync()
    {
        var mainWindow = await GetMainWindowAsync();
        if (!ReferenceEquals(mainWindow, _lastKnownMainWindow))
        {
            _lastKnownMainWindow = mainWindow;

            MainWindowChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
