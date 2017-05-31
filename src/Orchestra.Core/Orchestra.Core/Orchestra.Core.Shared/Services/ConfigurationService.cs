// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Globalization;
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

    /// <summary>
    /// Custom implementation to work around https://github.com/Catel/Catel/issues/1029, can be changed
    /// once upgraded to Catel v5
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IFileService _fileService;
        private readonly ISerializationManager _serializationManager;
        private readonly IObjectConverterService _objectConverterService;

        private string _localConfigFilePath;
        private string _roamingConfigFilePath;

        private DynamicConfiguration _localConfiguration;
        private DynamicConfiguration _roamingConfiguration;

        private bool _suspendNotifications = false;
        private bool _hasPendingNotifications = false;
        //private bool _isFailedToInitialize;

        #region Constructors
        public ConfigurationService(ISerializationManager serializationManager, IObjectConverterService objectConverterService,
            IFileService fileService)
        {
            Argument.IsNotNull(() => serializationManager);
            Argument.IsNotNull(() => objectConverterService);
            Argument.IsNotNull(() => fileService);

            _serializationManager = serializationManager;
            _objectConverterService = objectConverterService;
            _fileService = fileService;

            // Use temporary base initialization, InitializeAsync will override later
            _localConfigFilePath = Path.Combine(Path.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserLocal), "configuration.xml");

            try
            {
                if (File.Exists(_localConfigFilePath))
                {
                    using (var fileStream = new FileStream(_localConfigFilePath, FileMode.Open))
                    {
                        _localConfiguration = ModelBase.Load<DynamicConfiguration>(fileStream, SerializationMode.Xml, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load local configuration, using default settings");
            }

            if (_localConfiguration == null)
            {
                _localConfiguration = new DynamicConfiguration();
            }

            _roamingConfigFilePath = Path.Combine(Path.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming), "configuration.xml");

            try
            {
                if (File.Exists(_roamingConfigFilePath))
                {
                    using (var fileStream = new FileStream(_roamingConfigFilePath, FileMode.Open))
                    {
                        _roamingConfiguration = ModelBase.Load<DynamicConfiguration>(fileStream, SerializationMode.Xml, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load roaming configuration, using default settings");
            }

            if (_roamingConfiguration == null)
            {
                _roamingConfiguration = new DynamicConfiguration();
            }

#pragma warning disable 4014
            InitializeAsync();
#pragma warning restore 4014
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the configuration has changed.
        /// </summary>
        public event EventHandler<ConfigurationChangedEventArgs> ConfigurationChanged;
        #endregion

        #region Methods
        /// <summary>
        /// Suspends the notifications of this service until the returned object is disposed.
        /// </summary>
        /// <returns>IDisposable.</returns>
        public IDisposable SuspendNotifications()
        {
            return new DisposableToken<ConfigurationService>(this,
                x =>
                {
                    x.Instance._suspendNotifications = true;
                },
                x =>
                {
                    x.Instance._suspendNotifications = false;
                    if (x.Instance._hasPendingNotifications)
                    {
                        x.Instance.RaiseConfigurationChanged(ConfigurationContainer.Roaming, string.Empty, string.Empty);
                        x.Instance._hasPendingNotifications = false;
                    }
                });
        }

        /// <summary>
        /// Gets the configuration value.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value. Will be returned if the value cannot be found.</param>
        /// <returns>The configuration value.</returns>
        /// <exception cref="ArgumentException">The <paramref name="key" /> is <c>null</c> or whitespace.</exception>
        [ObsoleteEx(ReplacementTypeOrMember = "GetValue<T>(ConfigurationContainer, string, T)", TreatAsErrorFromVersion = "2.0", RemoveInVersion = "3.0")]
        public T GetValue<T>(string key, T defaultValue = default(T))
        {
            return GetValue(ConfigurationContainer.Roaming, key, defaultValue);
        }

        /// <summary>
        /// Gets the configuration value.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value. Will be returned if the value cannot be found.</param>
        /// <returns>The configuration value.</returns>
        /// <exception cref="ArgumentException">The <paramref name="key" /> is <c>null</c> or whitespace.</exception>
        public T GetValue<T>(ConfigurationContainer container, string key, T defaultValue = default(T))
        {
            Argument.IsNotNullOrWhitespace("key", key);

            key = GetFinalKey(key);

            try
            {
                if (!ValueExists(container, key))
                {
                    return defaultValue;
                }

                var value = GetValueFromStore(container, key);
                if (value == null)
                {
                    return defaultValue;
                }

                // ObjectConverterService doesn't support object, but just return the value as is
                if (typeof(T) == typeof(object))
                {
                    return (T)(object)value;
                }

                return (T)_objectConverterService.ConvertFromStringToObject(value, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, $"Failed to retrieve configuration value '{container}.{key}', returning default value");

                return defaultValue;
            }
        }

        /// <summary>
        /// Sets the configuration value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentException">The <paramref name="key"/> is <c>null</c> or whitespace.</exception>
        [ObsoleteEx(ReplacementTypeOrMember = "SetValue(ConfigurationContainer, string, object)", TreatAsErrorFromVersion = "2.0", RemoveInVersion = "3.0")]
        public void SetValue(string key, object value)
        {
            SetValue(ConfigurationContainer.Roaming, key, value);
        }

        /// <summary>
        /// Sets the configuration value.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentException">The <paramref name="key" /> is <c>null</c> or whitespace.</exception>
        public void SetValue(ConfigurationContainer container, string key, object value)
        {
            Argument.IsNotNullOrWhitespace("key", key);

            var originalKey = key;
            key = GetFinalKey(key);

            var stringValue = _objectConverterService.ConvertFromObjectToString(value, CultureInfo.InvariantCulture);
            var existingValue = GetValueFromStore(container, key);

            SetValueToStore(container, key, stringValue);

            if (!string.Equals(stringValue, existingValue))
            {
                RaiseConfigurationChanged(container, originalKey, value);
            }
        }

        /// <summary>
        /// Determines whether the specified value is available.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified value is available; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">The <paramref name="key"/> is <c>null</c> or whitespace.</exception>
        [ObsoleteEx(ReplacementTypeOrMember = "IsValueAvailable(ConfigurationContainer, string)", TreatAsErrorFromVersion = "2.0", RemoveInVersion = "3.0")]
        public bool IsValueAvailable(string key)
        {
            return IsValueAvailable(ConfigurationContainer.Roaming, key);
        }

        /// <summary>
        /// Determines whether the specified value is available.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified value is available; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">The <paramref name="key" /> is <c>null</c> or whitespace.</exception>
        public bool IsValueAvailable(ConfigurationContainer container, string key)
        {
            Argument.IsNotNullOrWhitespace("key", key);

            key = GetFinalKey(key);

            return ValueExists(container, key);
        }

        /// <summary>
        /// Initializes the value by setting the value to the <paramref name="defaultValue" /> if the value does not yet exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <exception cref="ArgumentException">The <paramref name="key"/> is <c>null</c> or whitespace.</exception>
        [ObsoleteEx(ReplacementTypeOrMember = "InitializeValue(ConfigurationContainer, string, object)", TreatAsErrorFromVersion = "2.0", RemoveInVersion = "3.0")]
        public void InitializeValue(string key, object defaultValue)
        {
            InitializeValue(ConfigurationContainer.Roaming, key, defaultValue);
        }

        /// <summary>
        /// Initializes the value by setting the value to the <paramref name="defaultValue" /> if the value does not yet exist.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <exception cref="ArgumentException">The <paramref name="key" /> is <c>null</c> or whitespace.</exception>
        public void InitializeValue(ConfigurationContainer container, string key, object defaultValue)
        {
            Argument.IsNotNullOrWhitespace("key", key);

            if (!IsValueAvailable(container, key))
            {
                SetValue(container, key, defaultValue);
            }
        }

        /// <summary>
        /// Determines whether the specified key value exists in the configuration.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the value exists, <c>false</c> otherwise.</returns>
        protected virtual bool ValueExists(ConfigurationContainer container, string key)
        {
            var settings = GetSettingsContainer(container);
            return settings.IsConfigurationValueSet(key);
        }

        protected virtual string GetValueFromStore(ConfigurationContainer container, string key)
        {
            var settings = GetSettingsContainer(container);
            return settings.GetConfigurationValue<string>(key, string.Empty);
        }

        protected virtual void SetValueToStore(ConfigurationContainer container, string key, string value)
        {
            var settings = GetSettingsContainer(container);

            if (!settings.IsConfigurationValueSet(key))
            {
                settings.RegisterConfigurationKey(key);
            }

            settings.SetConfigurationValue(key, value);

            string fileName = string.Empty;

            switch (container)
            {
                case ConfigurationContainer.Local:
                    fileName = _localConfigFilePath;
                    break;

                case ConfigurationContainer.Roaming:
                    fileName = _roamingConfigFilePath;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("container");
            }

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

                //_isFailedToInitialize = true;
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

        /// <summary>
        /// Gets the final key. This method allows customization of the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        protected virtual string GetFinalKey(string key)
        {
            key = key.Replace(" ", "_");

            return key;
        }

        private void RaiseConfigurationChanged(ConfigurationContainer container, string key, object value)
        {
            if (_suspendNotifications)
            {
                _hasPendingNotifications = true;
                return;
            }

            ConfigurationChanged.SafeInvoke(this, () => new ConfigurationChangedEventArgs(container, key, value));
        }

        /// <summary>
        /// Gets the settings container for this platform
        /// </summary>
        /// <param name="container">The settings container.</param>
        /// <returns>The settings container.</returns>
        protected DynamicConfiguration GetSettingsContainer(ConfigurationContainer container)
        {
            DynamicConfiguration settings = null;

            switch (container)
            {
                case ConfigurationContainer.Local:
                    settings = _localConfiguration;
                    break;

                case ConfigurationContainer.Roaming:
                    settings = _roamingConfiguration;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("container");
            }

            return settings;
        }
        #endregion
    }
}