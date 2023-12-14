namespace Orchestra.Services
{
    using System.Threading.Tasks;
    using System.Windows;

    public class SplashScreenService : ISplashScreenService
    {
        /// <summary>
        /// Creates the splash screen.
        /// </summary>
        /// <returns>The window.</returns>
        public virtual async Task<Window> CreateSplashScreenAsync()
        {
            var splashScreen = new Views.SplashScreen();

            return splashScreen;
        }
    }
}
