namespace Orchestra.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Services;
    using Services;

    /// <summary>
    /// The splash screen view model.
    /// </summary>
    public class SplashScreenViewModel : ViewModelBase
    {
        private readonly IAboutInfoService _aboutInfoService;
        private readonly ILanguageService _languageService;

        public SplashScreenViewModel(IAboutInfoService aboutInfoService, ILanguageService languageService)
        {
            ArgumentNullException.ThrowIfNull(aboutInfoService);
            ArgumentNullException.ThrowIfNull(languageService);

            _aboutInfoService = aboutInfoService;
            _languageService = languageService;
        }

        public static bool IsActive { get; private set; }

        public Uri? CompanyLogoForSplashScreenUri { get; private set; }

        public string? Company { get; private set; }

        public string? ProducedBy { get; private set; }

        public string? Version { get; private set; }        

        protected override async Task InitializeAsync()
        {
            IsActive = true;

            await base.InitializeAsync();

            var aboutInfo = await _aboutInfoService.GetAboutInfoAsync();

            Title = aboutInfo.Name;
            Company = aboutInfo.Company;
            CompanyLogoForSplashScreenUri = aboutInfo.CompanyLogoForSplashScreenUri;
            ProducedBy = string.Format(_languageService.GetString("Orchestra_ProducedBy"), Company);
            Version = aboutInfo.DisplayVersion;
        }

        protected override Task OnClosedAsync(bool? result)
        {
            IsActive = false;

            return base.OnClosedAsync(result);
        }
    }
}
