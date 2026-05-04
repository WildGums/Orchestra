namespace Orchestra.ViewModels;

using System;
using System.Threading.Tasks;
using Catel.Logging;
using Catel.MVVM;
using Microsoft.Extensions.Logging;
using Services;

public class ShellViewModel : FeaturedViewModelBase
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(ShellViewModel));

    private readonly ITaskRunnerService _taskRunnerService;

    public ShellViewModel(IServiceProvider serviceProvider, ITaskRunnerService taskRunnerService,
        ICommandManager commandManager, IShellConfigurationService shellConfigurationService)
        : base(serviceProvider)
    {
        _taskRunnerService = taskRunnerService;

        Run = new TaskCommand(serviceProvider, OnRunExecuteAsync, OnRunCanExecute);

        commandManager.RegisterCommand("Runner.Run", Run, this);

        DeferValidationUntilFirstSaveCall = shellConfigurationService.DeferValidationUntilFirstSaveCall;

        Title = taskRunnerService.Title;
        taskRunnerService.TitleChanged += (sender, args) => Title = taskRunnerService.Title;
    }

    public bool IsRunning { get; private set; }

    public object? ConfigurationContext { get; set; }

    /// <summary>
    /// Gets the Run command.
    /// </summary>
    public TaskCommand Run { get; private set; }

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
    private async Task OnRunExecuteAsync()
    {
        Validate(true);

        if (HasErrors)
        {
            Logger.LogWarning("There are errors that need to be fixed, please do that before running.");

            var validationSummary = this.GetValidationSummary(true);
            foreach (var error in validationSummary.FieldErrors)
            {
                Logger.LogWarning("  * {ErrorMessage}", error.Message);
            }

            foreach (var error in validationSummary.BusinessRuleErrors)
            {
                Logger.LogWarning("  * {ErrorMessage}", error.Message);
            }

            return;
        }

        IsRunning = true;

        try
        {
            await Task.Run(() => _taskRunnerService.RunAsync(ConfigurationContext));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to execute the command");
        }
        finally
        {
            IsRunning = false;
        }
    }
}
