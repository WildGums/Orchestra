namespace Orchestra.Services
{
    using System;
    
    public static class ICommandInfoServiceExtensions
    {
        public static void UpdateCommandInfo(this ICommandInfoService commandInfoService, string commandName, Action<ICommandInfo> commandInfoUpdateCallback)
        {
            ArgumentNullException.ThrowIfNull(commandInfoService);
            ArgumentNullException.ThrowIfNull(commandName);
            ArgumentNullException.ThrowIfNull(commandInfoUpdateCallback);

            var commandInfo = commandInfoService.GetCommandInfo(commandName);
            commandInfoUpdateCallback(commandInfo);
        }
    }
}
