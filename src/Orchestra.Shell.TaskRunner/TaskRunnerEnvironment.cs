namespace Orchestra
{
    using System.IO;

    public static class TaskRunnerEnvironment
    {
        public static readonly string CurrentLogFileName = Path.Combine(Catel.IO.Path.GetApplicationDataDirectory(), "current.log");
    }
}
