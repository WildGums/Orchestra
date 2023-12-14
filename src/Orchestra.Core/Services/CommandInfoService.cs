namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using Catel.MVVM;

    public class CommandInfoService : ICommandInfoService
    {
        private readonly ICommandManager _commandManager;
        private readonly Dictionary<string, ICommandInfo> _commandInfo = new Dictionary<string, ICommandInfo>();

        public CommandInfoService(ICommandManager commandManager)
        {
            ArgumentNullException.ThrowIfNull(commandManager);

            _commandManager = commandManager;
        }
        
        public ICommandInfo GetCommandInfo(string commandName)
        {
            ArgumentNullException.ThrowIfNull(commandName);

            if (!_commandInfo.ContainsKey(commandName))
            {
                var inputGesture = _commandManager.GetInputGesture(commandName);

                _commandInfo[commandName] = new CommandInfo(commandName, inputGesture);
            }

            return _commandInfo[commandName];
        }

        public void UpdateCommandInfo(string commandName, ICommandInfo commandInfo)
        {
            ArgumentNullException.ThrowIfNull(commandName);
            ArgumentNullException.ThrowIfNull(commandInfo);

            _commandInfo[commandName] = commandInfo;
        }

        public void Invalidate()
        {
            _commandInfo.Clear();
        }
    }
}
