namespace Orchestra.Services
{
    using System.Threading.Tasks;
    using System.Windows;

    public interface ISplashScreenService
    {
        /// <summary>
        /// Creates the splash screen.
        /// </summary>
        /// <returns>The window.</returns>
        Task<Window> CreateSplashScreenAsync();
    }
}
