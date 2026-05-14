namespace Orchestra.Logging;

using System.IO;
using Catel.Services;

public class LogDirectoryProvider
{
    private readonly IAppDataService _appDataService;

    public LogDirectoryProvider(IAppDataService appDataService)
    {
        _appDataService = appDataService;
    }

    public string ProvideDirectory()
    {
        var directory = Path.Combine(_appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming), "log");

        return directory;
    }
}
