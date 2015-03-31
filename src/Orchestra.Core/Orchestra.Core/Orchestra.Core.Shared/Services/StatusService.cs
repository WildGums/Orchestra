// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Timers;
    using System.Windows.Threading;
    using Catel;
    using Catel.Logging;
    using Orchestra.Logging;

    public class StatusService : IStatusService
    {
        #region Fields
        private readonly IStatusFilterService _statusFilterService;

        private IStatusRepresenter _statusRepresenter;
        private string _lastStatus;
        #endregion

        #region Constructors
        public StatusService(IStatusFilterService statusFilterService)
        {
            Argument.IsNotNull(() => statusFilterService);

            _statusFilterService = statusFilterService;

            var statusLogListener = new StatusLogListener(this);

            LogManager.AddListener(statusLogListener);
        }
        #endregion

        #region IStatusService Members
        public void UpdateStatus(string status)
        {
            var finalStatus = _statusFilterService.GetStatus(status);
            if (string.IsNullOrWhiteSpace(finalStatus))
            {
                return;
            }

            SetStatus(status);

            _lastStatus = finalStatus;

            var resetTimer = new DispatcherTimer();
            resetTimer.Interval = TimeSpan.FromSeconds(8);;
            resetTimer.Tick += OnResetTimerTick;
            resetTimer.Tag = finalStatus;
            resetTimer.Start();
        }
        #endregion

        #region Methods
        public void Initialize(IStatusRepresenter statusRepresenter)
        {
            Argument.IsNotNull(() => statusRepresenter);

            _statusRepresenter = statusRepresenter;
        }

        private void OnResetTimerTick(object sender, EventArgs e)
        {
            var timer = (DispatcherTimer)sender;

            string finalStatus = (string)timer.Tag;

            timer.Stop();
            timer.Tick -= OnResetTimerTick;

            if (string.Equals(_lastStatus, finalStatus))
            {
                SetStatus("Ready");
            }
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

            _statusRepresenter.UpdateStatus(status);
        }
        #endregion
    }
}