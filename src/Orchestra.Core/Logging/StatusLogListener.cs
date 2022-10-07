namespace Orchestra.Logging
{
    using System;
    using Catel.Logging;
    using Services;

    /// <summary>
    /// Status log listener.
    /// </summary>
    public class StatusLogListener : Catel.Logging.StatusLogListener
    {
        private readonly IStatusService _statusService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusLogListener"/> class.
        /// </summary>
        /// <param name="statusService">The status service.</param>
        public StatusLogListener(IStatusService statusService)
        {
            ArgumentNullException.ThrowIfNull(statusService);

            _statusService = statusService;

            IgnoreCatelLogging = true;
            IsDebugEnabled = false;
            IsInfoEnabled = false;
            IsWarningEnabled = false;
            IsErrorEnabled = false;
            IsStatusEnabled = true;
        }

        protected override void Write(ILog log, string message, LogEvent logEvent, object? extraData, LogData? logData, DateTime time)
        {
            _statusService.UpdateStatus(message);
        }
    }
}
