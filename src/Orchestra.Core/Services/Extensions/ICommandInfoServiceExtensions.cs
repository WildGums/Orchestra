// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandInfoServiceExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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
            ArgumentNullException.ThrowIfNull(commandInfoService);
            ArgumentNullException.ThrowIfNull(commandName);
            ArgumentNullException.ThrowIfNull(commandInfoUpdateCallback);

            var commandInfo = commandInfoService.GetCommandInfo(commandName);
            commandInfoUpdateCallback(commandInfo);
        }
    }
}