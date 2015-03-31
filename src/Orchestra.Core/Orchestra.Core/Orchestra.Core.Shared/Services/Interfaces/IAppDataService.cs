// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAppDataService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAppDataService
    {
        #region Methods
        Task<bool> BackupUserData();
        Task<bool> OpenApplicationDataDirectory();
        #endregion

        void DeleteUserData();
        List<string> ExclusionFilters { get; }
        string ApplicationDataDirectory { get; }
    }
}