namespace Orchestra;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Catel;
using Catel.Configuration;
using Catel.Logging;
using Catel.Reflection;
using Catel.Services;
using MethodTimer;
using Microsoft.Extensions.Logging;
using Orc.FileSystem;

public class ConfigurationBackupService : IConfigurationBackupService
{
    private readonly ILogger<ConfigurationBackupService> _logger;
    private readonly IConfigurationService _configurationService;
    private readonly IAppDataService _appDataService;
    private readonly IFileService _fileService;
    private readonly IDirectoryService _directoryService;

    public ConfigurationBackupService(ILogger<ConfigurationBackupService> logger, IConfigurationService configurationService, 
        IAppDataService appDataService, IFileService fileService, IDirectoryService directoryService)
    {
        _logger = logger;
        _configurationService = configurationService;
        _appDataService = appDataService;
        _fileService = fileService;
        _directoryService = directoryService;
    }

    public int NumberOfBackups { get; protected set; } = 10;

    public string BackupTimeStampFormat { get; set; } = "{0:yyyyMMdd_HHmmssfff}";

    [Time]
    public virtual async Task BackupAsync()
    {
        try
        {
            var configurationServiceType = typeof(Catel.Configuration.ConfigurationService);

            // Get configuration paths
            var roamingConfigFilePathField = configurationServiceType.GetFieldEx("_roamingConfigFilePath", true, false);
            if (roamingConfigFilePathField is null)
            {
                throw _logger.LogErrorAndCreateException<OrchestraException>($"Roaming config file path field not found on the configuration service");
            }

            var localConfigFilePathField = configurationServiceType.GetFieldEx("_localConfigFilePath", true, false);
            if (localConfigFilePathField is null)
            {
                throw _logger.LogErrorAndCreateException<OrchestraException>($"Local config file path field not found on the configuration service");
            }

            var roamingConfigFilePath = roamingConfigFilePathField.GetValue(_configurationService)?.ToString();
            if (!string.IsNullOrWhiteSpace(roamingConfigFilePath))
            {
                await BackupConfigurationAsync(roamingConfigFilePath, Catel.IO.ApplicationDataTarget.UserRoaming);
            }

            var localConfigFilePath = localConfigFilePathField.GetValue(_configurationService)?.ToString();
            if (!string.IsNullOrWhiteSpace(localConfigFilePath))
            {
                await BackupConfigurationAsync(localConfigFilePath, Catel.IO.ApplicationDataTarget.UserLocal);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create configuration backup");
            throw;
        }
    }

    protected virtual async Task BackupConfigurationAsync(string configurationFilePath, Catel.IO.ApplicationDataTarget applicationDataTarget)
    {
        if (!_fileService.Exists(configurationFilePath))
        {
            _logger.LogDebug($"Configuration file not found on path {configurationFilePath}, skipping backup");
            return;
        }

        var configBackupFolderPath = Path.Combine(_appDataService.GetApplicationDataDirectory(applicationDataTarget), "backup", "config");

        _directoryService.Create(configBackupFolderPath);

        // Save current configuration
        var process = Process.GetCurrentProcess();
        var dateTime = process.StartTime;

        var targetFileName = Path.Combine(configBackupFolderPath, $"configuration.{string.Format(BackupTimeStampFormat, dateTime)}.json");
        if (_fileService.Exists(targetFileName))
        {
            // Already created
            return;
        }

        _logger.LogInformation($"Creating configuration backup, {applicationDataTarget}");

        _fileService.Copy(configurationFilePath, targetFileName, true);

        _logger.LogInformation($"Created configuration backup, {applicationDataTarget}");

        var backupConfigurationFiles = _directoryService.GetFiles(configBackupFolderPath, "configuration*").OrderBy(f => f).ToList();
        if (backupConfigurationFiles.Count >= NumberOfBackups)
        {
            // Remove oldest backup
            var filesToRemoveCount = 1 + NumberOfBackups - backupConfigurationFiles.Count;

            for (int i = 0; i < filesToRemoveCount; i++)
            {
                var backupFilePath = Path.Combine(configBackupFolderPath, backupConfigurationFiles[i]);
                _fileService.Delete(backupFilePath);
            }
        }
    }
}
