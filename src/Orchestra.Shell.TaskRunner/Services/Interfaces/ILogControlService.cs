namespace Orchestra.Services
{
    using Catel.Logging;

    /// <summary>
    /// Log control service.
    /// </summary>
    public interface ILogControlService
    {
        LogEvent SelectedLevel { get; set; }

        void Clear();
    }
}
