namespace Orchestra.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IManageAppDataService
    {
        List<string> ExclusionFilters { get; }

        Task<bool> BackupUserDataAsync(Catel.IO.ApplicationDataTarget applicationDataTarget);
        Task DeleteUserDataAsync(Catel.IO.ApplicationDataTarget applicationDataTarget);

        bool OpenApplicationDataDirectory(Catel.IO.ApplicationDataTarget applicationDataTarget);
    }
}
