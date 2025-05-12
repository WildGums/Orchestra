namespace Orchestra.Services
{
    using System;
    using Catel.Logging;
    using Orc.Controls.Services;

    public class SplashScreenStatusService : ISplashScreenStatusService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private IStatusRepresenter? _statusRepresenter;

        public SplashScreenStatusService()
        {

        }

        public void UpdateStatus(string status)
        {
            SetStatus(status);
        }

        public void Initialize(IStatusRepresenter statusRepresenter)
        {
            ArgumentNullException.ThrowIfNull(statusRepresenter);

            _statusRepresenter = statusRepresenter;
        }

        private void SetStatus(string status)
        {
            if (!string.IsNullOrWhiteSpace(status))
            {
                var statusLines = status.Split(new[] { "\n", "\r\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                if (statusLines.Length > 0)
                {
                    status = statusLines[0];
                }
            }

            Log.Info($"Updating status to: {status}");

            _statusRepresenter?.UpdateStatus(status);
        }
    }
}
