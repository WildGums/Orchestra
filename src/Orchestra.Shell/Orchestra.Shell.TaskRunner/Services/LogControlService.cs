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

    public class LogControlService : ILogControlService
    {
        private readonly TraceOutputControl _traceOutputControl;

        #region Constructors
        public LogControlService(TraceOutputControl traceOutputControl)
        {
            Argument.IsNotNull("traceOutputControl", traceOutputControl);

            _traceOutputControl = traceOutputControl;
        }
        #endregion

        public LogEvent SelectedLevel
        {
            get { return _traceOutputControl.SelectedLevel; }
            set { _traceOutputControl.SelectedLevel = value; }
        }

        public void Clear()
        {
            _traceOutputControl.Clear();
        }
    }
}