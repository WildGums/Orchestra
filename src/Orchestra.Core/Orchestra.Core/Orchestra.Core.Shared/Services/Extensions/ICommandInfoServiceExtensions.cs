// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandInfoServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using Catel;
    using Models;

    public static class ICommandInfoServiceExtensions
    {
        public static void UpdateCommandInfo(this ICommandInfoService commandInfoService, string commandName, Action<ICommandInfo> commandInfoUpdateCallback)
        {
            Argument.IsNotNull(() => commandInfoService);
            Argument.IsNotNull(() => commandName);
            Argument.IsNotNull(() => commandInfoUpdateCallback);

            var commandInfo = commandInfoService.GetCommandInfo(commandName);
            commandInfoUpdateCallback(commandInfo);
        }
    }
}