// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAppDataService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Collections.Generic;

    public interface IAppDataService
    {
        #region Methods
        bool BackupUserData();
        bool OpenApplicationDataDirectory();
        #endregion

        void DeleteUserData();
        List<string> ExclusionFilters { get; }
        string ApplicationDataDirectory { get; }
    }
}