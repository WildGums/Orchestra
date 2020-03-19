// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppDataService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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
    using Orc.FileSystem;

    public class ManageAppDataService : Orchestra.Services.IManageAppDataService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ISaveFileService _saveFileService;
        private readonly IProcessService _processService;
        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;
        private readonly IAppDataService _appDataService;

        public ManageAppDataService(ISaveFileService saveFileService, IProcessService processService,
            IDirectoryService directoryService, IFileService fileService, IAppDataService appDataService)
        {
            Argument.IsNotNull(() => saveFileService);
            Argument.IsNotNull(() => processService);
            Argument.IsNotNull(() => directoryService);
            Argument.IsNotNull(() => fileService);
            Argument.IsNotNull(() => appDataService);

            _saveFileService = saveFileService;
            _processService = processService;
            _directoryService = directoryService;
            _fileService = fileService;
            _appDataService = appDataService;

            ExclusionFilters = new List<string>(new[]
            {
                "licenseinfo.xml",
                "*.log"
            });
        }

        public List<string> ExclusionFilters { get; private set; }

        public bool OpenApplicationDataDirectory(Catel.IO.ApplicationDataTarget applicationDataTarget)
        {
            Log.Info("Opening data directory");

            var applicationDataDirectory = _appDataService.GetApplicationDataDirectory(applicationDataTarget);
            _processService.StartProcess(applicationDataDirectory);

            return true;
        }

        protected virtual bool MatchesFilters(IEnumerable<string> filters, string fileName)
        {
            return FilterHelper.MatchesFilters(filters, fileName);
        }

        public async Task DeleteUserDataAsync(Catel.IO.ApplicationDataTarget applicationDataTarget)
        {
            var applicationDataDirectory = _appDataService.GetApplicationDataDirectory(applicationDataTarget);

            Log.Debug("Deleting user data from '{0}'", applicationDataDirectory);

            var exclusionFilters = ExclusionFilters;

            var allFiles = _directoryService.GetFiles(applicationDataDirectory, "*.*", SearchOption.AllDirectories);
            foreach (var fileName in allFiles)
            {
                if (!MatchesFilters(exclusionFilters, fileName))
                {
                    _fileService.Delete(fileName);
                }
            }
        }

        public async Task<bool> BackupUserDataAsync(Catel.IO.ApplicationDataTarget applicationDataTarget)
        {
            var assembly = AssemblyHelper.GetEntryAssembly();
            var applicationDataDirectory = _appDataService.GetApplicationDataDirectory(applicationDataTarget);

            var result = await _saveFileService.DetermineFileAsync(new DetermineSaveFileContext
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = string.Format("{0} backup {1}.zip", assembly.Title(), DateTime.Now.ToString("yyyyMMdd hhmmss")),
                Filter = "Zip files|*.zip"
            });

            if (!result.Result)
            {
                return false;
            }

            var zipFileName = result.FileName;

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
