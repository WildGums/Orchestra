// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Path = Catel.IO.Path;

    public static class LogFilePrefixes
    {
        /// <summary>
        /// The crashreport prefix.
        /// </summary>
        public static readonly string CrashReport = "Crashreport";

        /// <summary>
        /// The entry assembly name prefix.
        /// </summary>
        public static readonly string EntryAssemblyName = Catel.Reflection.AssemblyHelper.GetEntryAssembly().GetName().Name;

        /// <summary>
        /// The 'Log' file log prefix.
        /// </summary>
        public static readonly string Log = "Log";

        /// <summary>
        /// All file log prefixes
        /// </summary>
        public static readonly string[] All = { EntryAssemblyName, CrashReport, Log };
    }

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
            AddFileLogListener(LogFilePrefixes.Log);
        }

        /// <summary>
        /// Adds a file log listener for an unhandled exception.
        /// </summary>
        /// <param name="ex">The unhandled exception.</param>
        [ObsoleteEx(TreatAsErrorFromVersion = "5.0", RemoveInVersion = "6.0", ReplacementTypeOrMember = "AddLogListenerForUnhandledExceptionAsync")]
        public static async void AddLogListenerForUnhandledException(Exception ex)
        {
            await AddLogListenerForUnhandledExceptionAsync(ex);
        }

        /// <summary>
        /// Adds a file log listener for an unhandled exception.
        /// </summary>
        /// <param name="ex">The unhandled exception.</param>
        public static async Task AddLogListenerForUnhandledExceptionAsync(Exception ex)
        {
            AddFileLogListener(LogFilePrefixes.CrashReport);

            Log.Error(ex, "Application crashed");

            await LogManager.FlushAllAsync();
        }

        public static ILogListener CreateFileLogListener(string prefix)
        {
            Argument.IsNotNull(() => prefix);

            var directory = GetLogDirectory();

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var fileName = Path.Combine(directory, prefix + "_{Date}_{Time}_{ProcessId}");
            var fileLogListener = new Orchestra.Logging.FileLogListener(fileName, 10 * 1024);

            return fileLogListener;
        }

        public static void CleanUpAllLogTypeFiles(bool keepCleanInRealTime = false)
        {
            var directory = GetLogDirectory();

            foreach (var prefix in LogFilePrefixes.All)
            {
                var filter = prefix + "*.log";
                CleanUpLogFiles(directory, filter);
                if (keepCleanInRealTime)
                {
                    ConfigureFileSystemWatcher(directory, filter);
                }
            }
        }

        private static string GetLogDirectory()
        {
            var appDataService = ServiceLocator.Default.ResolveType<IAppDataService>();
            return Path.Combine(appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming), "log");
        }

        private static void AddFileLogListener(string prefix)
        {
            var fileLogListener = CreateFileLogListener(prefix);

            LogManager.AddListener(fileLogListener);

            Log.LogProductInfo();
            Log.LogDeviceInfo();
        }

        private static void ConfigureFileSystemWatcher(string directory, string filter)
        {
            var fileSystemWatcher = new FileSystemWatcher(directory, filter)
            {
                EnableRaisingEvents = true
            };

            fileSystemWatcher.Created += (sender, args) => { CleanUpLogFiles(directory, filter); };
        }

        private static void CleanUpLogFiles(string directory, string filter)
        {
            try
            {
                var files = Directory.GetFiles(directory, filter).Select(file => new { FileName = file, LastWriteTime = File.GetLastWriteTime(file)} ).ToList();

                files.Sort((f1, f2) => f1.LastWriteTime.CompareTo(f2.LastWriteTime));

                int i = 0;
                while (i < files.Count && (files[i].LastWriteTime < DateTime.Now.AddDays(-7) || files.Count - i > 10))
                {
                    File.Delete(files[i].FileName);
                    i++;
                }
            }
            catch (Exception)
            {
                // Ignore
            }
        }
    }
}
