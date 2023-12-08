namespace Orchestra.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Catel.Configuration;
    using Catel.IO;
    using Catel.IoC;
    using Catel.Services;
    using NUnit.Framework;
    using Orc.FileSystem;
    using Orchestra.Services;

    [TestFixture]
    public class ConfigurationBackupServiceFacts
    {
        private TestConfigurationBackupService _testConfigurationBackupService = null;
       
        [Explicit("Because we use current process start date/time for file formatting, we cannot run this multiple times in the same instance of a test")]
        [TestCase(
            @"C:\Users\{0}\AppData\Roaming\WildGums\Orchestra.Examples.Ribbon.Fluent\testconfiguration.xml",
            @"C:\Users\{0}\AppData\Local\WildGums\Orchestra.Examples.Ribbon.Fluent\testconfiguration.xml",
            15
        )]
        public async Task TestBackupConfigurationAsyncCleanupExpectedAsync(string roamingConfigPath, string localConfigPath, int numberOfRuns)
        {
            var sl = ServiceLocator.Default;
            var configurationService = sl.ResolveType<IConfigurationService>();
            var appDataService = sl.ResolveType<IAppDataService>();
            var fileService = sl.ResolveType<IFileService>();
            var directoryService = sl.ResolveType<IDirectoryService>();

            // Initialize paths
            roamingConfigPath = string.Format(roamingConfigPath, Environment.UserName);
            localConfigPath = string.Format(localConfigPath, Environment.UserName);

            // Pre-initialization
            if (_testConfigurationBackupService is null)
            {
                _testConfigurationBackupService = new TestConfigurationBackupService(configurationService, appDataService, fileService, directoryService);
            }

            var roamingDirectory = Path.GetDirectoryName(roamingConfigPath);
            directoryService.Create(roamingDirectory);

            // Prepare files for case
            using (fileService.Create(roamingConfigPath))
            {
            }

            var localDirectory = Path.GetDirectoryName(localConfigPath);
            directoryService.Create(localDirectory);

            using (fileService.Create(localConfigPath))
            { 
            }

            // Testing case
            for (int i = 0; i < numberOfRuns; i++)
            {
                await _testConfigurationBackupService.TestBackupConfigurationAsync(roamingConfigPath, ApplicationDataTarget.UserRoaming);
                await _testConfigurationBackupService.TestBackupConfigurationAsync(localConfigPath, ApplicationDataTarget.UserLocal);
            }

            // Check file count
            var countInRoaming = directoryService.GetFiles(string.Format(@"C:\Users\{0}\AppData\Roaming\Microsoft Corporation\Microsoft.TestHost\backup\config", Environment.UserName),
                "configuration*").Count();
            var countInLocal = directoryService.GetFiles(string.Format(@"C:\Users\{0}\AppData\Local\Microsoft Corporation\Microsoft.TestHost\backup\config", Environment.UserName),
                "configuration*").Count();

            Assert.That(_testConfigurationBackupService.NumberOfBackups, Is.EqualTo(countInRoaming));
            Assert.That(_testConfigurationBackupService.NumberOfBackups, Is.EqualTo(countInLocal));
        }

        public class TestConfigurationBackupService : ConfigurationBackupService
        {
            public TestConfigurationBackupService(IConfigurationService configurationService, IAppDataService appDataService, IFileService fileService, IDirectoryService directoryService)
                : base(configurationService, appDataService, fileService, directoryService)
            {
            }

            public async Task TestBackupConfigurationAsync(string configurationFilePath, ApplicationDataTarget applicationDataTarget)
            {
                await BackupConfigurationAsync(configurationFilePath, applicationDataTarget);
            }

            protected override async Task BackupConfigurationAsync(string configurationFilePath, ApplicationDataTarget applicationDataTarget)
            {
                await base.BackupConfigurationAsync(configurationFilePath, applicationDataTarget);
            }
        }
    }
}
