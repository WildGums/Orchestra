namespace Orchestra.Services
{
    using System.Threading.Tasks;

    public interface IConfigurationBackupService
    {
        string BackupTimeStampFormat { get; set; }
        int NumberOfBackups { get; }

        void Backup();
        Task BackupAsync();
    }
}
