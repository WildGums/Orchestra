namespace Orchestra.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Orchestra.Services;

    public class ThirdPartyNoticesViewModel : ViewModelBase
    {
        private readonly IAboutInfoService _aboutInfoService;
        private readonly IThirdPartyNoticesService _thirdPartyNoticesService;

        public ThirdPartyNoticesViewModel(IAboutInfoService aboutInfoService,
            IThirdPartyNoticesService thirdPartyNoticesService)
        {
            ArgumentNullException.ThrowIfNull(aboutInfoService);
            ArgumentNullException.ThrowIfNull(thirdPartyNoticesService);

            _aboutInfoService = aboutInfoService;
            _thirdPartyNoticesService = thirdPartyNoticesService;

            Title = LanguageHelper.GetRequiredString("Orchestra_ThirdPartyNotices_Title");
        }

        public string Explanation { get; private set; }

        public List<ThirdPartyNotice> ThirdPartyNotices { get; private set; }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            var aboutInfo = await _aboutInfoService.GetAboutInfoAsync();
            var explanation = LanguageHelper.GetString("Orchestra_ThirdPartyNotices_Explanation");
            Explanation = string.Format(explanation, aboutInfo.Company, aboutInfo.ProductName);

            ThirdPartyNotices = await _thirdPartyNoticesService.GetThirdPartyNoticesAsync();
        }
    }
}
