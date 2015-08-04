﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogControlService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using Catel;
    using Catel.Logging;
    using Orc.LogViewer;

    public class LogControlService : ILogControlService
    {
        private readonly AdvancedLogViewerControl _traceOutputControl;

        #region Constructors
        public LogControlService(AdvancedLogViewerControl traceOutputControl)
        {
            Argument.IsNotNull(() => traceOutputControl);

            _traceOutputControl = traceOutputControl;
        }
        #endregion

        public LogEvent SelectedLevel
        {
            get { return _traceOutputControl.Level; }
            set { _traceOutputControl.Level = value; }
        }

        public void Clear()
        {
            _traceOutputControl.Clear();
        }
    }
}