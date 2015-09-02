// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMahAppsService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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