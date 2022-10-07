﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogControlService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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
            ArgumentNullException.ThrowIfNull(traceOutputControl);

            _traceOutputControl = traceOutputControl;
        }
        #endregion

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
