// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using Catel;
    using Catel.Logging;
    using Orchestra.Logging;

    public class StatusService : IStatusService
    {
        #region Fields
        private IStatusRepresenter _statusRepresenter;
        private string _lastStatus;
        #endregion

        #region Constructors
        public StatusService()
        {
            var statusLogListener = new StatusLogListener(this);

            LogManager.AddListener(statusLogListener);
        }
        #endregion

        #region IStatusService Members
        public void UpdateStatus(string statusFormat, params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(statusFormat))
            {
                statusFormat = string.Empty;
            }

            if (parameters.Length > 0)
            {
                statusFormat = string.Format(statusFormat, parameters);
            }

            SetStatus(statusFormat);

            _lastStatus = statusFormat;

            var duration = TimeSpan.FromSeconds(8);
            var resetTimer = new System.Timers.Timer(duration.TotalMilliseconds);
            resetTimer.Elapsed += (sender, e) =>
            {
                resetTimer.Stop();

                if (string.Equals(_lastStatus, statusFormat))
                {
                    SetStatus("Ready");
                }
            };
            resetTimer.Start();
        }
        #endregion

        #region Methods
        public void Initialize(IStatusRepresenter statusRepresenter)
        {
            Argument.IsNotNull(() => statusRepresenter);

            _statusRepresenter = statusRepresenter;
        }

        private void SetStatus(string status)
        {
            if (!string.IsNullOrWhiteSpace(status))
            {
                var statusLines = status.Split(new [] { "\n", "\r\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                if (statusLines.Length > 0)
                {
                    status = statusLines[0];
                }
            }

            _statusRepresenter.UpdateStatus(status);
        }
        #endregion
    }
}