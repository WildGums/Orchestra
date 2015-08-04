// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppDataService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.Reflection;
    using Catel.Services;
    using Ionic.Zip;

    public class AppDataService : IAppDataService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IMessageService _messageService;
        private readonly ISaveFileService _saveFileService;
        private readonly IProcessService _processService;

        public AppDataService(IMessageService messageService, ISaveFileService saveFileService, IProcessService processService)
        {
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => saveFileService);
            Argument.IsNotNull(() => processService);

            _messageService = messageService;
            _saveFileService = saveFileService;
            _processService = processService;

            ExclusionFilters = new List<string>(new []
            {
                "licenseinfo.xml",
                "*.log"
            });

            var applicationDataDirectory = Catel.IO.Path.GetApplicationDataDirectory();
            if (!Directory.Exists(applicationDataDirectory))
            {
                Directory.CreateDirectory(applicationDataDirectory);
            }

            ApplicationDataDirectory = applicationDataDirectory;
        }

        public string ApplicationDataDirectory { get; private set; }

        public List<string> ExclusionFilters { get; private set; }

        public bool OpenApplicationDataDirectory()
        {
            Log.Info("Opening data directory");

            var applicationDataDirectory = ApplicationDataDirectory;
            _processService.StartProcess(applicationDataDirectory);

            return true;
        }

        protected virtual bool MatchesFilters(IEnumerable<string> filters, string fileName)
        {
            return FilterHelper.MatchesFilters(filters, fileName);
        }

        public void DeleteUserData()
        {
            var applicationDataDirectory = ApplicationDataDirectory;

            Log.Debug("Deleting user data from '{0}'", applicationDataDirectory);

            var exclusionFilters = ExclusionFilters;

            var allFiles = Directory.GetFiles(applicationDataDirectory, "*.*", SearchOption.AllDirectories);
            foreach (var fileName in allFiles)
            {
                if (!MatchesFilters(exclusionFilters, fileName))
                {
                    File.Delete(fileName);
                }
            }
        }

        public bool BackupUserData()
        {
            var assembly = AssemblyHelper.GetEntryAssembly();
            var applicationDataDirectory = Catel.IO.Path.GetApplicationDataDirectory();

            _saveFileService.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _saveFileService.FileName = string.Format("{0} backup {1}.zip", assembly.Title(), DateTime.Now.ToString("yyyyMMdd hhmmss"));
            _saveFileService.Filter = "Zip files|*.zip";

            if (!_saveFileService.DetermineFile())
            {
                return false;
            }

            var zipFileName = _saveFileService.FileName;

            Log.Debug("Writing zip file to '{0}'", zipFileName);

            using (var zipFile = new ZipFile())
            {
                zipFile.AddDirectory(applicationDataDirectory, null);
                zipFile.Save(zipFileName);
            }

            return true;
        }
    }
}