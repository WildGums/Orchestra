namespace Orchestra.Services
{
    using System.Threading.Tasks;

    public interface ICloseApplicationService
    {
        [ObsoleteEx(TreatAsErrorFromVersion = "5.0", RemoveInVersion = "6.0", ReplacementTypeOrMember = "CloseAsync")]
        void Close();

        Task CloseAsync();
    }
}
