// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Path = Catel.IO.Path;

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
        public static void AddLogListenerForUnhandledException(Exception ex)
        {
            AddFileLogListener("Crashreport");

            Log.Error(ex, "Application crashed");

            LogManager.FlushAll();
        }

        public static ILogListener CreateFileLogListener(string prefix)
        {
            Argument.IsNotNull(() => prefix);

            var directory = Path.Combine(Path.GetApplicationDataDirectory(), "log");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var fileName = Path.Combine(directory, prefix + "_{Date}_{Time}_{ProcessId}");
            var fileLogListener = new FileLogListener(fileName, 10 * 1024);

            return fileLogListener;
        }

        private static void AddFileLogListener(string prefix)
        {
            var fileLogListener = CreateFileLogListener(prefix);

            LogManager.AddListener(fileLogListener);

            Log.LogProductInfo();
            Log.LogDeviceInfo();
        }
    }
}