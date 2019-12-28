// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAppDataService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IManageAppDataService
    {
        #region Methods
        Task<bool> BackupUserDataAsync(Catel.IO.ApplicationDataTarget applicationDataTarget);
        bool OpenApplicationDataDirectory(Catel.IO.ApplicationDataTarget applicationDataTarget);
        #endregion

        Task DeleteUserDataAsync(Catel.IO.ApplicationDataTarget applicationDataTarget);
        List<string> ExclusionFilters { get; }
    }
}
