namespace Orchestra.Logging
{
    using System;
    using System.Reflection;
    using Catel;
    using Catel.Logging;

    /// <summary>
    /// Special purpose file logger that also writes Catel argument logging. 
    /// <remarks>
    /// Will be used until Catel 5.0 has been released where it will always include argument logging, even when Catel logging is being ignored.
    /// </remarks>
    /// </summary>
    public class FileLogListener : Catel.Logging.FileLogListener
    {
        private static readonly Type ArgumentType = typeof(Argument);

        public FileLogListener(Assembly? assembly = null)
            : base(assembly)
        {
        }

        public FileLogListener(string filePath, int maxSizeInKiloBytes, Assembly? assembly = null)
            : base(filePath, maxSizeInKiloBytes, assembly)
        {
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
    }
}
