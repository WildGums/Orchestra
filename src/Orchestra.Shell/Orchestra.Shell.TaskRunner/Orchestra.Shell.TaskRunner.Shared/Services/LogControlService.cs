// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogControlService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using Catel;
    using Catel.Logging;
    using Catel.Windows.Controls;
    using Catel.Windows.Threading;
    using Orc.Controls;

    public class LogControlService : ILogControlService
    {
        private readonly LogViewerControl _traceOutputControl;

        #region Constructors
        public LogControlService(LogViewerControl traceOutputControl)
        {
            Argument.IsNotNull(() => traceOutputControl);

            _traceOutputControl = traceOutputControl;
            _traceOutputControl.Dispatcher.BeginInvoke(() =>
            {
                _traceOutputControl.ShowInfo = true;
                _traceOutputControl.ShowError = false;
                _traceOutputControl.ShowDebug = false;
                _traceOutputControl.ShowWarning = false;
            });
        }
        #endregion

        public LogEvent SelectedLevel
        {
            get
            {
                LogEvent logEvent = 0;

                if (_traceOutputControl.ShowDebug)
                {
                    logEvent |= LogEvent.Debug;
                }

                if (_traceOutputControl.ShowError)
                {
                    logEvent |= LogEvent.Error;
                }

                if (_traceOutputControl.ShowInfo)
                {
                    logEvent |= LogEvent.Info;
                }

                if (_traceOutputControl.ShowWarning)
                {
                    logEvent |= LogEvent.Warning;
                }

                return logEvent;
            }
            set
            {
                _traceOutputControl.ShowDebug = value.HasFlag(LogEvent.Debug);
                _traceOutputControl.ShowError = value.HasFlag(LogEvent.Error);
                _traceOutputControl.ShowInfo = value.HasFlag(LogEvent.Info);
                _traceOutputControl.ShowWarning = value.HasFlag(LogEvent.Warning);
            }
        }

        public void Clear()
        {
            _traceOutputControl.Clear();
        }
    }
}