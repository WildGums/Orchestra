// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using Catel;
    using Catel.IO;
    using Catel.Logging;

    /// <summary>
    /// Helper class for logging.
    /// </summary>
    public static class LogHelper
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Adds a file log listener.
        /// </summary>
        public static void AddFileLogListener()
        {
            AddFileLogListener("Log");
        }

        /// <summary>
        /// Adds a file log listener for an unhandled exception.
        /// </summary>
        /// <param name="ex">The unhandled exception.</param>
        public static async void AddLogListenerForUnhandledException(Exception ex)
        {
            AddFileLogListener("Crashreport");

            Log.Error(ex, "Application crashed");

            await LogManager.FlushAllAsync();
        }

        private static void AddFileLogListener(string prefix)
        {
            Argument.IsNotNull(() => prefix);

            var fileName = Path.Combine(Path.GetApplicationDataDirectory(), prefix + "_{Date}_{Time}_{ProcessId}");
            var fileLogListener = new FileLogListener(fileName, 10 * 1024);

            LogManager.AddListener(fileLogListener);

            Log.LogProductInfo();
            Log.LogDeviceInfo();
        }
    }
}