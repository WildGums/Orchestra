namespace Orchestra
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;

    public static class LogFilePrefixes
    {
        /// <summary>
        /// The crashreport prefix.
        /// </summary>
        public static readonly string CrashReport = "Crashreport";

        /// <summary>
        /// The entry assembly name prefix.
        /// </summary>
        public static readonly string EntryAssemblyName = Catel.Reflection.AssemblyHelper.GetRequiredEntryAssembly().GetName().Name ?? string.Empty;

        /// <summary>
        /// The 'Log' file log prefix.
        /// </summary>
        public static readonly string Log = "Log";

        /// <summary>
        /// All file log prefixes
        /// </summary>
        public static readonly string[] All =
        {
            EntryAssemblyName,
            CrashReport,
            Log
        };
    }

    /// <summary>
    /// Helper class for logging.
    /// </summary>
    public static class LogHelper
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Maximum Log file size in KBs
        /// </summary>
        public static int MaxFileLogSize { get; set; } = 10 * 1024;

        public static int MaxLogFileArchiveDays { get; set; } = 14;

        public static int MaxLogFileArchiveFilesCount { get; set; } = 20;

        /// <summary>
        /// Adds a file log listener.
        /// </summary>
        public static void AddFileLogListener(string? prefix = null)
        {
            var fileLogListener = CreateFileLogListener(prefix) as Catel.Logging.FileLogListener;
            if (fileLogListener is null)
            {
                return;
            }

            if (File.Exists(fileLogListener.FilePath))
            {
                // Already creating a log here, no need to do it again
                return;
            }

            LogManager.AddListener(fileLogListener);

            Log.LogProductInfo();
            Log.LogDeviceInfo();
        }

        /// <summary>
        /// Adds a file log listener for an unhandled exception.
        /// </summary>
        /// <param name="ex">The unhandled exception.</param>
        public static async Task AddLogListenerForUnhandledExceptionAsync(Exception ex)
        {
            ArgumentNullException.ThrowIfNull(ex);

            AddFileLogListener(LogFilePrefixes.CrashReport);

            Log.Error(ex, "Application crashed");

            await LogManager.FlushAllAsync();
        }

        public static ILogListener CreateFileLogListener(string? prefix = null)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                prefix = LogFilePrefixes.EntryAssemblyName;
            }
            
            var directory = GetLogDirectory();

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var fileName = Path.Combine(directory, prefix + "_{Date}_{Time}_{ProcessId}");
            var fileLogListener = new Orchestra.Logging.FileLogListener(fileName, MaxFileLogSize);

            return fileLogListener;
        }

        public static void CleanUpAllLogTypeFiles(bool keepCleanInRealTime = false)
        {
            var directory = GetLogDirectory();

            foreach (var prefix in LogFilePrefixes.All)
            {
                var filter = prefix + "*.log";

                CleanUpLogFiles(directory, filter, MaxLogFileArchiveDays, MaxLogFileArchiveFilesCount);

                if (keepCleanInRealTime)
                {
                    ConfigureFileSystemWatcher(directory, filter, (args) => CleanUpLogFiles(directory, filter, MaxLogFileArchiveDays, MaxLogFileArchiveFilesCount));
                }
            }
        }

        private static string GetLogDirectory()
        {
            var appDataService = ServiceLocator.Default.ResolveRequiredType<IAppDataService>();

            var directory = Path.Combine(appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming), "log");

            Directory.CreateDirectory(directory);

            return directory;
        }

        private static void ConfigureFileSystemWatcher(string directory, string filter, Action<FileSystemEventArgs> pathCreatedHandler)
        {
#pragma warning disable IDISP001 // Dispose created.
            var fileSystemWatcher = new FileSystemWatcher(directory, filter)
            {
                EnableRaisingEvents = true
            };
#pragma warning restore IDISP001 // Dispose created.

            fileSystemWatcher.Created += (sender, args) => pathCreatedHandler(args);
        }

        private static void CleanUpLogFiles(string directory, string filter, int maxLogFileArchiveDays, int maxLogFileArchiveFilesCount)
        {
            try
            {
                var files = Directory.GetFiles(directory, filter).Select(file => new { FileName = file, LastWriteTime = File.GetLastWriteTime(file) }).ToList();

                files.Sort((f1, f2) => f1.LastWriteTime.CompareTo(f2.LastWriteTime));

                int i = 0;
                while (i < files.Count && (files[i].LastWriteTime < DateTime.Now.AddDays(-1 * maxLogFileArchiveDays) || files.Count - i > maxLogFileArchiveFilesCount))
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
