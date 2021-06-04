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
       
        [TestCase(
            @"C:\Users\JustAnotherUser\AppData\Roaming\WildGums\Orchestra.Examples.Ribbon.Fluent\testconfiguration.xml",
            @"C:\Users\JustAnotherUser\AppData\Local\WildGums\Orchestra.Examples.Ribbon.Fluent\testconfiguration.xml",
            15
        )]
        public async Task TestBackupConfigurationAsyncCleanupExpectedAsync(string roamingConfigPath, string localConfigPath, int numberOfRuns)
        {
            var sl = ServiceLocator.Default;
            var configurationService = sl.ResolveType<IConfigurationService>();
            var appDataService = sl.ResolveType<IAppDataService>();
            var fileService = sl.ResolveType<IFileService>();
            var directoryService = sl.ResolveType<IDirectoryService>();

            // Pre-initialization
            if (_testConfigurationBackupService is null)
            {
                _testConfigurationBackupService = new TestConfigurationBackupService(configurationService, appDataService, fileService, directoryService);
            }

            // Prepare files for case
            using (fileService.Create(roamingConfigPath))
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
            var countInRoaming = directoryService.GetFiles(@"C:\Users\JustAnotherUser\AppData\Roaming\Microsoft Corporation\Microsoft.TestHost\backup\config", "configuration*").Count();
            var countInLocal = directoryService.GetFiles(@"C:\Users\JustAnotherUser\AppData\Local\Microsoft Corporation\Microsoft.TestHost\backup\config", "configuration*").Count();

            Assert.AreEqual(countInRoaming, _testConfigurationBackupService.NumberOfBackups);
            Assert.AreEqual(countInLocal, _testConfigurationBackupService.NumberOfBackups);
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
