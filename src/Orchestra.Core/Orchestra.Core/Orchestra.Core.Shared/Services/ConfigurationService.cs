// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Configuration;
    using Catel.Data;
    using Catel.Logging;
    using Catel.Runtime.Serialization;
    using Catel.Services;
    using Orc.FileSystem;
    using Path = Catel.IO.Path;

    public class ConfigurationService : Catel.Configuration.ConfigurationService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IFileService _fileService;
        private string _localConfigFilePath;
        private string _roamingConfigFilePath;

        private DynamicConfiguration _localConfiguration;
        private DynamicConfiguration _roamingConfiguration;

        private bool _isFailedToInitialize;

        #region Constructors
        public ConfigurationService(ISerializationManager serializationManager, IObjectConverterService objectConverterService, IFileService fileService) 
            : base(serializationManager, objectConverterService)
        {
            Argument.IsNotNull(() => fileService);

            _fileService = fileService;

#pragma warning disable 4014
            InitializeAsync();
#pragma warning restore 4014
        }    
        #endregion

        protected override string GetValueFromStore(ConfigurationContainer container, string key)
        {
            if (_isFailedToInitialize)
            {
                return base.GetValueFromStore(container, key);
            }

            var settings = GetConfigurationContainer(container);
            if (settings == null)
            {
                // in case if configuration container still not loaded, use the one loaded in base class
                return base.GetValueFromStore(container, key);
            }

            return settings.GetConfigurationValue<string>(key, string.Empty);
        }

        protected override void SetValueToStore(ConfigurationContainer container, string key, string value)
        {
            if (_isFailedToInitialize)
            {
                base.SetValueToStore(container, key, value);

                return;
            }

            var settings = GetConfigurationContainer(container);

            if (!settings.IsConfigurationValueSet(key))
            {
                settings.RegisterConfigurationKey(key);
            }

            settings.SetConfigurationValue(key, value);

            var fileName = GetConfigurationFileName(container);

#pragma warning disable 4014
            SaveConfigurationAsync(fileName, settings);
#pragma warning restore 4014
        }


        private async Task InitializeAsync()
        {
            try
            {
                _localConfigFilePath = Path.Combine(Path.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserLocal), "configuration.xml");
                _localConfiguration = await LoadConfigurationAsync(_localConfigFilePath);

                _roamingConfigFilePath = Path.Combine(Path.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming), "configuration.xml");
                _roamingConfiguration = await LoadConfigurationAsync(_roamingConfigFilePath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to initialize ConfigurationService");

                _isFailedToInitialize = true;
            }
        }

        private async Task SaveConfigurationAsync(string filePath, DynamicConfiguration settings)
        {
            try
            {
                using (var fileLocker = new FileLocker(null))
                {
                    await fileLocker.LockFilesAsync(filePath);

                    using (var fileStream = _fileService.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    {
                        var serializer = SerializationFactory.GetXmlSerializer();
                        serializer.Serialize(settings, fileStream, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to save configuration file '{filePath}'");
            }
        }

        private async Task<DynamicConfiguration> LoadConfigurationAsync(string filePath)
        {
            if (!_fileService.Exists(filePath))
            {
                Log.Error($"Failed to load configuration, file '{filePath}' does not exists");

                return new DynamicConfiguration();
            }

            DynamicConfiguration result;

            using (var fileLocker = new FileLocker(null))
            {
                await fileLocker.LockFilesAsync(filePath);

                try
                {
                    using (var fileStream = _fileService.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        result = ModelBase.Load<DynamicConfiguration>(fileStream, SerializationMode.Xml, null);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Failed to load configuration file '{filePath}'");

                    result = new DynamicConfiguration();
                }
            }

            return result;
        }

        private DynamicConfiguration GetConfigurationContainer(ConfigurationContainer container)
        {
            DynamicConfiguration settings;

            switch (container)
            {
                case ConfigurationContainer.Local:
                    settings = _localConfiguration;
                    break;

                case ConfigurationContainer.Roaming:
                    settings = _roamingConfiguration;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(container));
            }

            return settings;
        }

        private string GetConfigurationFileName(ConfigurationContainer container)
        {
            string fileName;

            switch (container)
            {
                case ConfigurationContainer.Local:
                    fileName = _localConfigFilePath;
                    break;

                case ConfigurationContainer.Roaming:
                    fileName = _roamingConfigFilePath;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(container));
            }
            return fileName;
        }
    }
}