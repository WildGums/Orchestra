namespace Orchestra.Logging;

using Catel.Logging;
using Catel.Reflection;
using Microsoft.Extensions.Logging;

public static class ILoggerExtensions
{
    public static void LogApplicationInfo<TProgram>(this ILogger logger)
    {
        logger.LogInformation(string.Empty);
        logger.LogInformation(string.Empty);
        logger.LogInformation(string.Empty);
        logger.LogInformationHeading1($"Component version: {typeof(TProgram).Assembly.InformationalVersion()}");

        logger.LogInformation(".NET version: {Version}", System.Environment.Version);
        logger.LogInformation(".NET runtime version: {Version}", System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);

        logger.LogInformation(string.Empty);
        logger.LogInformation(string.Empty);
        logger.LogInformation(string.Empty);
    }
}
