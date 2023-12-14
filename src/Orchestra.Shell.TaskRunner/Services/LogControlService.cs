namespace Orchestra.Services
{
    using System;
    using Catel.Logging;
    using Orc.LogViewer;

    public class LogControlService : ILogControlService
    {
        private readonly AdvancedLogViewerControl _traceOutputControl;

        public LogControlService(AdvancedLogViewerControl traceOutputControl)
        {
            ArgumentNullException.ThrowIfNull(traceOutputControl);

            _traceOutputControl = traceOutputControl;
        }

        public LogEvent SelectedLevel
        {
            get { return _traceOutputControl.Level; }
#pragma warning disable WPF0035 // Use SetValue in setter.
            set { _traceOutputControl.SetCurrentValue(AdvancedLogViewerControl.LevelProperty, value); }
#pragma warning restore WPF0035 // Use SetValue in setter.
        }

        public void Clear()
        {
            _traceOutputControl.Clear();
        }
    }
}
