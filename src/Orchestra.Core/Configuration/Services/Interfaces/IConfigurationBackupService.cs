namespace Orchestra;

using System.Threading.Tasks;

public interface IConfigurationBackupService
{
    string BackupTimeStampFormat { get; set; }
    int NumberOfBackups { get; }

    Task BackupAsync();
}
