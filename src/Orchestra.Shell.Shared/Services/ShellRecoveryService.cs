namespace Orchestra;

using System.Threading.Tasks;
using System.Windows;
using Catel.Reflection;
using Catel.Services;

public partial class ShellRecoveryService : IShellRecoveryService
{
    private readonly IMessageService _messageService;
    private readonly IEntryAssemblyResolver _entryAssemblyResolver;

    public ShellRecoveryService(IMessageService messageService, IEntryAssemblyResolver entryAssemblyResolver)
    {
        _messageService = messageService;
        _entryAssemblyResolver = entryAssemblyResolver;
    }

    public async Task StartRecoveryAsync(ShellRecoveryContext shellRecoveryContext)
    {
        var entryAssembly = _entryAssemblyResolver.Resolve();
        var assemblyTitle = entryAssembly.Title();

        await _messageService.ShowErrorAsync(string.Format("An unexpected error occurred while starting {0}. Unfortunately it needs to be closed.\n\nPlease try restarting the application. If this error keeps coming up while starting the application, please contact support.", assemblyTitle), string.Format("Failed to start {0}", assemblyTitle));

        Application.Current.Shutdown(-1);
    }
}
