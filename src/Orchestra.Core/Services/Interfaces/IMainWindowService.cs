namespace Orchestra.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;

    public interface IMainWindowService
    {
        event EventHandler<EventArgs> MainWindowChanged;

        Task<Window> GetMainWindowAsync();
    }
}
