// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationSynchronizerBase.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Configuration
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Configuration;
    using Catel.IoC;
    using Catel.Logging;

    public abstract class ConfigurationSynchronizerBase<T> : INeedCustomInitialization
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private T _lastKnownValue;

        protected ConfigurationSynchronizerBase(string key, T defaultValue, IConfigurationService configurationService)
        {
            Argument.IsNotNullOrWhitespace(() => key);
            Argument.IsNotNull(() => configurationService);

            ApplyAtStartup = true;
            Key = key;
            DefaultValue = defaultValue;
            ConfigurationService = configurationService;

            ConfigurationService.ConfigurationChanged += OnConfigurationChanged;
        }

        protected IConfigurationService ConfigurationService { get; private set; }

        protected string Key { get; private set; }

        protected T DefaultValue { get; private set; }

        protected bool ApplyAtStartup { get; set; }

        protected virtual void ApplyConfiguration(T value)
        {
        }

        protected virtual async Task ApplyConfigurationAsync(T value)
        {
            ApplyConfiguration(value);
        }

        protected abstract string GetStatus(T value);

        async void INeedCustomInitialization.Initialize()
        {
            // Note: important to apply first, otherwise the check for values might be equal (which we don't want during first apply)
            if (ApplyAtStartup)
            {
                await ApplyConfigurationAsync(true);
            }

            _lastKnownValue = ConfigurationService.GetValue(Key, DefaultValue);
        }

        private async void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            if (e.IsConfigurationKey(Key))
            {
                await ApplyConfigurationAsync();
            }
        }

        private async Task ApplyConfigurationAsync(bool force = false)
        {
            var value = ConfigurationService.GetValue(Key, DefaultValue);
            if (!force && ObjectHelper.AreEqual(value, _lastKnownValue))
            {
                return;
            }

            _lastKnownValue = value;

            await ApplyConfigurationAsync(value);

            var status = GetStatus(value);
            if (!string.IsNullOrWhiteSpace(status))
            {
                Log.Info(status);
            }
        }
    }
}