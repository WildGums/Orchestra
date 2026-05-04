namespace Orchestra.Behaviors;

using System;
using System.Windows;
using Catel.IoC;
using Catel.Logging;
using Catel.Services;
using Catel.Windows.Interactivity;
using Microsoft.Extensions.Logging;
using Orc.Controls;

public partial class RememberWindowSize : BehaviorBase<Window>
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(RememberWindowSize));

    private readonly IAppDataService _appDataService;

    public RememberWindowSize(IAppDataService appDataService)
    {
        _appDataService = appDataService;
    }

    public bool MakeWindowResizable
    {
        get { return (bool)GetValue(MakeWindowResizableProperty); }
        set { SetValue(MakeWindowResizableProperty, value); }
    }

    public static readonly DependencyProperty MakeWindowResizableProperty = DependencyProperty.Register(nameof(MakeWindowResizable),
        typeof(bool), typeof(RememberWindowSize), new PropertyMetadata(true));


    public bool RememberWindowState
    {
        get { return (bool)GetValue(RememberWindowStateProperty); }
        set { SetValue(RememberWindowStateProperty, value); }
    }

    public static readonly DependencyProperty RememberWindowStateProperty = DependencyProperty.Register(nameof(RememberWindowState),
        typeof(bool), typeof(RememberWindowSize), new PropertyMetadata(true));


    protected override void OnAssociatedObjectLoaded()
    {
        base.OnAssociatedObjectLoaded();

        var window = AssociatedObject;
        var windowType = window.GetType().Name;

        if (MakeWindowResizable && window.ResizeMode == ResizeMode.NoResize)
        {
            Logger.LogDebug("Setting window ResizeMode to CanResize and SizeToContent to Manual of '{WindowType}'", windowType);

            window.SetCurrentValue(Window.SizeToContentProperty, SizeToContent.Manual);
            window.SetCurrentValue(Window.ResizeModeProperty, ResizeMode.CanResize);
        }

        _appDataService.LoadWindowSize(window, RememberWindowState);

        switch (window.WindowStartupLocation)
        {
            case WindowStartupLocation.Manual:
                break;

            case WindowStartupLocation.CenterScreen:
                window.CenterWindowToScreen();
                break;

            case WindowStartupLocation.CenterOwner:
                window.CenterWindowToParent();
                break;

            default:
                throw Logger.LogErrorAndCreateException(_ => new ArgumentOutOfRangeException(nameof(window.WindowStartupLocation)), string.Empty);
        }

        window.Closed += OnWindowClosed;
    }

    protected override void OnAssociatedObjectUnloaded()
    {
        var window = AssociatedObject;

        window.Closed -= OnWindowClosed;

        base.OnAssociatedObjectUnloaded();
    }

    private void OnWindowClosed(object? sender, EventArgs e)
    {
        var window = AssociatedObject;
        _appDataService.SaveWindowSize(window);
    }
}
