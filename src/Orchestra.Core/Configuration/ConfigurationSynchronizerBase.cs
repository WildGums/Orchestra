// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationSynchronizerBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Configuration
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Configuration;
    using Catel.IoC;
    using Catel.Logging;

    public abstract class ConfigurationSynchronizerBase<T> : INeedCustomInitialization
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private T _lastKnownValue;

        protected ConfigurationSynchronizerBase(string key, T defaultValue,  IConfigurationService configurationService)
            : this(key, defaultValue, ConfigurationContainer.Roaming, configurationService)
        {
        }

        protected ConfigurationSynchronizerBase(string key, T defaultValue, ConfigurationContainer container, IConfigurationService configurationService)
        {
            Argument.IsNotNullOrWhitespace(() => key);
            Argument.IsNotNull(() => configurationService);

            ApplyAtStartup = true;
            Key = key;
            Container = container;
            DefaultValue = defaultValue;
            ConfigurationService = configurationService;

            ConfigurationService.ConfigurationChanged += OnConfigurationChanged;
        }

        protected IConfigurationService ConfigurationService { get; private set; }

        protected string Key { get; private set; }

        protected ConfigurationContainer Container { get; private set; }

        protected T DefaultValue { get; private set; }

        protected bool ApplyAtStartup { get; set; }

        public virtual T GetCurrentValue()
        {
            var value = ConfigurationService.GetValue(Container, Key, DefaultValue);
            return value;
        }

        public virtual void ApplyConfiguration()
        {
            var value = ConfigurationService.GetValue(Container, Key, DefaultValue);

            ApplyConfiguration(value);
        }

        protected virtual void ApplyConfiguration(T value)
        {
        }

        protected virtual async Task ApplyConfigurationAsync(T value)
        {
            ApplyConfiguration(value);
        }

        protected abstract string GetStatus(T value);

#pragma warning disable AvoidAsyncVoid
        async void INeedCustomInitialization.Initialize()
#pragma warning restore AvoidAsyncVoid
        {
            // Note: important to apply first, otherwise the check for values might be equal (which we don't want during first apply)
            if (ApplyAtStartup)
            {
                await ApplyConfigurationInternalAsync(true);
            }

            _lastKnownValue = ConfigurationService.GetValue(Container, Key, DefaultValue);
        }

#pragma warning disable AvoidAsyncVoid
        private async void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
#pragma warning restore AvoidAsyncVoid
        {
            if (e.IsConfigurationKey(Key))
            {
                await ApplyConfigurationInternalAsync();
            }
        }

        private async Task ApplyConfigurationInternalAsync(bool force = false)
        {
            var value = GetCurrentValue();
            if (!force && ObjectHelper.AreEqual(value, _lastKnownValue))
            {
                return;
            }

            _lastKnownValue = value;

            try
            {
                await ApplyConfigurationAsync(value);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to apply configuration value for '{Key}'");
                throw;
            }

            var status = GetStatus(value);
            if (!string.IsNullOrWhiteSpace(status))
            {
                Log.Info(status);
            }
        }
    }
}