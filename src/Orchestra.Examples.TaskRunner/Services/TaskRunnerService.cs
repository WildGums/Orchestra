namespace Orchestra.Examples.TaskRunner.Services;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Catel.Services;
using Microsoft.Extensions.Logging;
using Models;
using Orchestra.Services;
using Views;

public class TaskRunnerService : ITaskRunnerService
{
    private readonly ILogger<TaskRunnerService> _logger;
    private readonly ILanguageService _languageService;

    public TaskRunnerService(ILogger<TaskRunnerService> logger, ILanguageService languageService)
    {
        _logger = logger;
        _languageService = languageService;
    }

    public string Title
    {
        get { return _languageService.GetRequiredString("Orchestra_Examples_TaskRunner_TaskRunnerService_Title"); }
    }

#pragma warning disable CS0067 // The event is never used - title is read-only from language service
    public event EventHandler? TitleChanged;
#pragma warning restore CS0067

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

        _logger.LogInformation("  Working directory => {WorkingDirectory}", settings.WorkingDirectory);
        _logger.LogInformation("  Output directory => {OutputDirectory}", settings.OutputDirectory);
        _logger.LogInformation("  Current time => {CurrentTime}", settings.CurrentTime);
        _logger.LogInformation("  Horizon start => {HorizonStart}", settings.HorizonStart);
        _logger.LogInformation("  Horizon end => {HorizonEnd}", settings.HorizonEnd);

        _logger.LogInformation("Sleeping to show long running action with blocking thread");

        Thread.Sleep(2500);

        _logger.LogInformation("Action is complete!");
    }

    public System.Windows.Size GetInitialWindowSize()
    {
        return System.Windows.Size.Empty;
    }

    public async Task<AboutInfo> GetAboutInfoAsync()
    {
        return new AboutInfo();
    }
}
