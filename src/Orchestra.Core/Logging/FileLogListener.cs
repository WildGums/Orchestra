namespace Orchestra.Logging
{
    using System;
    using System.Reflection;
    using Catel;
    using Catel.IO;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;

    /// <summary>
    /// Special purpose file logger that also writes Catel argument logging. 
    /// <remarks>
    /// Will be used until Catel 5.0 has been released where it will always include argument logging, even when Catel logging is being ignored.
    /// </remarks>
    /// </summary>
    public class FileLogListener : Catel.Logging.FileLogListener
    {
        private static readonly Type ArgumentType = typeof(Argument);

        private readonly IAppDataService _appDataService;

        public FileLogListener(IAppDataService appDataService, Assembly? assembly = null)
            : base(assembly)
        {
            _appDataService = appDataService;
        }

        public FileLogListener(IAppDataService appDataService, string filePath, int maxSizeInKiloBytes, Assembly? assembly = null)
            : base(filePath, maxSizeInKiloBytes, assembly)
        {
            _appDataService = appDataService;
        }

        protected override bool ShouldIgnoreLogMessage(ILog log, string message, LogEvent logEvent, object? extraData, LogData? logData, DateTime time)
        {
            // Ignore Catel logging that is not about Arguments
            if (log.IsCatelLogging && log.TargetType != ArgumentType)
            {
                return true;
            }

            return base.ShouldIgnoreLogMessage(log, message, logEvent, extraData, logData, time);
        }

        protected override string GetApplicationDataDirectory(ApplicationDataTarget target, string company, string product)
        {
            var baseDirectory = _appDataService.GetApplicationDataDirectory(target);
            var directory = baseDirectory;

            if (!string.IsNullOrWhiteSpace(company))
            {
                directory = System.IO.Path.Combine(directory, company);
            }

            if (!string.IsNullOrWhiteSpace(product))
            {
                directory = System.IO.Path.Combine(directory, product);
            }

            return directory;
        }
    }
}
