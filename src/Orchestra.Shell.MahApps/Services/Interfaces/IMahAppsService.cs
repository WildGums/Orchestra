// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMahAppsService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using MahApps.Metro.Controls;

    public interface IMahAppsService : IShellContentService, IAboutInfoService
    {
        WindowCommands GetRightWindowCommands();
    }
}