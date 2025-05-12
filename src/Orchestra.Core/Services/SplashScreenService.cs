namespace Orchestra.Services
{
    using System.Threading.Tasks;
    using System.Windows;

    public class SplashScreenService : ISplashScreenService
    {
        private readonly ISplashScreenStatusService _splashScreenStatusService;

        public SplashScreenService(ISplashScreenStatusService splashScreenStatusService)
        {
            _splashScreenStatusService = splashScreenStatusService;
        }

        /// <summary>
        /// Creates the splash screen.
        /// </summary>
        /// <returns>The window.</returns>
        public virtual async Task<Window> CreateSplashScreenAsync()
        {
            var splashScreen = new Views.SplashScreen();

            _splashScreenStatusService.Initialize(splashScreen);

            return splashScreen;
        }
    }
}
