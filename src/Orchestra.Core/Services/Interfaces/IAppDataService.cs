// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAppDataService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAppDataService
    {
        #region Methods
        Task<bool> BackupUserDataAsync();
        bool OpenApplicationDataDirectory();
        #endregion

        Task DeleteUserDataAsync();
        List<string> ExclusionFilters { get; }
        string ApplicationDataDirectory { get; }
    }
}