// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandInfoService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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