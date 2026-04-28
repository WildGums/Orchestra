namespace Orchestra;

using System;
using System.Windows.Input;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orchestra.Services;
using InputGesture = Catel.Windows.Input.InputGesture;

/// <summary>
/// Core module which allows the registration of default services in the service collection.
/// </summary>
public static partial class OrchestraShellTaskRunnerModule
{
    public static IServiceCollection AddOrchestraShellTaskRunner(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddSingleton<CommandInitializer>();

        serviceCollection.TryAddSingleton<IShellService, ShellService>();
        serviceCollection.TryAddSingleton<IShellRecoveryService, ShellRecoveryService>();
        serviceCollection.TryAddSingleton<IApplicationInitializationService, ApplicationInitializationServiceBase>();
        serviceCollection.TryAddSingleton<IShellConfigurationService, ShellConfigurationService>();
        serviceCollection.TryAddSingleton<IBusyIndicatorService, ProgressBusyIndicatorService>();
        serviceCollection.TryAddSingleton<IProgressBarProvider, ProgressBarProvider>();
        serviceCollection.TryAddSingleton<IXamlResourceService, XamlResourceService>();
        serviceCollection.TryAddSingleton<IAboutInfoService>(x => x.GetRequiredService<ITaskRunnerService>());

        serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orchestra.Shell.TaskRunner", "Orchestra.Properties", "Resources"));

        return serviceCollection;
    }

    private class CommandInitializer : IConstructAtStartup
    {
        public CommandInitializer(IServiceProvider serviceProvider, ICommandManager commandManager)
        {
            commandManager.CreateCommand("Help.About", throwExceptionWhenCommandIsAlreadyCreated: false);
            commandManager.CreateCommand("Groups.Run", new InputGesture(Key.R, ModifierKeys.Shift | ModifierKeys.Control));

            commandManager.CreateCommand("Runner.Run", new InputGesture(Key.F5), throwExceptionWhenCommandIsAlreadyCreated: false);
            commandManager.CreateCommand("Runner.ClearConsole", throwExceptionWhenCommandIsAlreadyCreated: false);
        }
    }
}
