namespace Orchestra.Services
{
    using System.Threading.Tasks;
    using MethodTimer;

    public interface IConfigurationBackupService
    {
        void Backup();
        Task BackupAsync();
    }
}
