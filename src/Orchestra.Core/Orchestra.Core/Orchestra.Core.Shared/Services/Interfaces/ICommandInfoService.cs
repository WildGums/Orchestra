// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandInfoService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using Models;

    public interface ICommandInfoService
    {
        ICommandInfo GetCommandInfo(string commandName);
        void UpdateCommandInfo(string commandName, ICommandInfo commandInfo);
    }
}