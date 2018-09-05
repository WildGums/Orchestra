namespace Orchestra.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Orchestra.Services;

    public class ThirdPartyNoticesViewModel : ViewModelBase
    {
        private readonly IThirdPartyNoticesService _thirdPartyNoticesService;

        public ThirdPartyNoticesViewModel(IAboutInfoService aboutInfoService,
            IThirdPartyNoticesService thirdPartyNoticesService)
        {
            Argument.IsNotNull(() => aboutInfoService);
            Argument.IsNotNull(() => thirdPartyNoticesService);

            _thirdPartyNoticesService = thirdPartyNoticesService;

            var aboutInfo = aboutInfoService.GetAboutInfo();

            Title = LanguageHelper.GetString("Orchestra_ThirdPartyNotices_Title");

            var explanation = LanguageHelper.GetString("Orchestra_ThirdPartyNotices_Explanation");
            Explanation = string.Format(explanation, aboutInfo.Company, aboutInfo.ProductName);
        }

        public string Explanation { get; private set; }

        public List<ThirdPartyNotice> ThirdPartyNotices { get; private set; }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            ThirdPartyNotices = _thirdPartyNoticesService.GetThirdPartyNotices();
        }
    }
}
