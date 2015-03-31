// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToggleConfigurationCommandContainerBase.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Configuration;
    using Catel.MVVM;

    public abstract class ToggleConfigurationCommandContainerBase : ToggleConfigurationCommandContainerBase<object>
    {
        protected ToggleConfigurationCommandContainerBase(string commandName, string configurationKey, bool defaultValue, ICommandManager commandManager, IConfigurationService configurationService)
            : base(commandName, configurationKey, defaultValue, commandManager, configurationService)
        {
        }
    }

    public abstract class ToggleConfigurationCommandContainerBase<TParameter> : ToggleConfigurationCommandContainerBase<TParameter, TParameter>
    {
        protected ToggleConfigurationCommandContainerBase(string commandName, string configurationKey, bool defaultValue, ICommandManager commandManager, IConfigurationService configurationService)
            : base(commandName, configurationKey, defaultValue, commandManager, configurationService)
        {
        }
    }

    public abstract class ToggleConfigurationCommandContainerBase<TExecuteParameter, TCanExecuteParameter> : CommandContainerBase<TExecuteParameter, TCanExecuteParameter>
    {
        private readonly string _configurationKey;
        private readonly bool _defaultValue;

        protected ToggleConfigurationCommandContainerBase(string commandName, string configurationKey, bool defaultValue, ICommandManager commandManager, IConfigurationService configurationService)
            : base(commandName, commandManager)
        {
            Argument.IsNotNullOrWhitespace(() => configurationKey);
            Argument.IsNotNull(() => configurationService);

            _configurationKey = configurationKey;
            _defaultValue = defaultValue;
            ConfigurationService = configurationService;
        }

        protected IConfigurationService ConfigurationService { get; private set; }

        protected override async Task ExecuteAsync(TExecuteParameter parameter)
        {
            var oldVersion = ConfigurationService.GetValue(_configurationKey, _defaultValue);
            ConfigurationService.SetValue(_configurationKey, !oldVersion);
        }
    }
}