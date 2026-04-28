namespace Orchestra.Examples.TaskRunner.Services;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Logging;
using Models;
using Orchestra.Services;
using Views;

public class TaskRunnerService : ITaskRunnerService
{
    private readonly ILogger<TaskRunnerService> _logger;

    private string _title = "Custom TaskRunner demo";

    public TaskRunnerService(ILogger<TaskRunnerService> logger)
    {
        _logger = logger;
    }

    public string Title
    {
        get { return _title; }
        set
        {
            _title = value;
            TitleChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler TitleChanged;

    public bool ShowCustomizeShortcutsButton { get { return true; }}

    public object? GetViewDataContext()
    {
        return new Settings();
    }

    public FrameworkElement? GetView()
    {
        return new SettingsView();
    }

    public async Task RunAsync(object? dataContext)
    {
        var settings = (Settings) dataContext;

        _logger.LogInformation("Running action with the following settings:");

        _logger.LogInformation("  Working directory => {0}", settings.WorkingDirectory);
        _logger.LogInformation("  Output directory => {0}", settings.OutputDirectory);
        _logger.LogInformation("  Current time => {0}", settings.CurrentTime);
        _logger.LogInformation("  Horizon start => {0}", settings.HorizonStart);
        _logger.LogInformation("  Horizon end => {0}", settings.HorizonEnd);

        _logger.LogInformation("Sleeping to show long running action with blocking thread");

        Thread.Sleep(2500);

        _logger.LogInformation("Action is complete!");
    }

    public Size GetInitialWindowSize()
    {
        return Size.Empty;
    }

    public async Task<AboutInfo> GetAboutInfoAsync()
    {
        return new AboutInfo();
    }
}
