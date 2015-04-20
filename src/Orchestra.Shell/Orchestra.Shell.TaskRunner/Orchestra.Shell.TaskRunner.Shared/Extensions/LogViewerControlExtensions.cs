// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerControlExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using Catel.Logging;
    using Orc.Controls;

    internal static class LogViewerControlExtensions
    {
        public static LogEvent GetLogEvent(this LogViewerControl logViewerControl)
        {
            LogEvent logEvent = 0;

            if (logViewerControl.ShowDebug)
            {
                logEvent |= LogEvent.Debug;
            }

            if (logViewerControl.ShowError)
            {
                logEvent |= LogEvent.Error;
            }

            if (logViewerControl.ShowInfo)
            {
                logEvent |= LogEvent.Info;
            }

            if (logViewerControl.ShowWarning)
            {
                logEvent |= LogEvent.Warning;
            }

            return logEvent;
        }

        public static void SetLogEvent(this LogViewerControl logViewerControl, LogEvent value)
        {
            logViewerControl.ShowDebug = value.HasFlag(LogEvent.Debug);
            logViewerControl.ShowError = value.HasFlag(LogEvent.Error);
            logViewerControl.ShowInfo = value.HasFlag(LogEvent.Info);
            logViewerControl.ShowWarning = value.HasFlag(LogEvent.Warning);
        }
    }
}