namespace Orchestra
{
    using Catel.IO;
    using Catel.IoC;
    using Catel.Services;
    using Path = System.IO.Path;

    public static class TaskRunnerEnvironment
    {
        public static readonly string CurrentLogFileName = Path.Combine(ServiceLocator.Default.ResolveRequiredType<IAppDataService>().GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming), "current.log");
    }
}
