namespace Orchestra;

using System.Threading.Tasks;
using System.Windows;
using Catel.Reflection;
using Catel.Services;

public partial class ShellRecoveryService : IShellRecoveryService
{
    private readonly IMessageService _messageService;
    private readonly ILanguageService _languageService;
    private readonly IEntryAssemblyResolver _entryAssemblyResolver;

    public ShellRecoveryService(IMessageService messageService, ILanguageService languageService,
        IEntryAssemblyResolver entryAssemblyResolver)
    {
        _messageService = messageService;
        _languageService = languageService;
        _entryAssemblyResolver = entryAssemblyResolver;
    }

    public async Task StartRecoveryAsync(ShellRecoveryContext shellRecoveryContext)
    {
        var entryAssembly = _entryAssemblyResolver.Resolve();
        var assemblyTitle = entryAssembly.Title();

        var errorMessage = string.Format(_languageService.GetRequiredString("Orchestra_ShellRecovery_UnexpectedError"), assemblyTitle);
        var errorTitle = string.Format(_languageService.GetRequiredString("Orchestra_ShellRecovery_FailedToStart"), assemblyTitle);

        await _messageService.ShowErrorAsync(errorMessage, errorTitle);

        Application.Current.Shutdown(-1);
    }
}
