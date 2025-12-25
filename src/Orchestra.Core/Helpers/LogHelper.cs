namespace Orchestra
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

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
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(LogHelper));

        /// <summary>
        /// Maximum Log file size in KBs
        /// </summary>
        public static int MaxFileLogSize { get; set; } = 10 * 1024;

        public static int MaxLogFileArchiveDays { get; set; } = 14;

        public static int MaxLogFileArchiveFilesCount { get; set; } = 20;

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
            var appDataService = IoCContainer.ServiceProvider.GetRequiredService<IAppDataService>();

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
