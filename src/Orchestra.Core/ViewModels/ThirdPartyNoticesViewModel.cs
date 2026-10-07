namespace Orchestra.ViewModels;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catel;
using Catel.MVVM;
using Catel.ThirdPartyNotices;
using Orchestra;

public partial class ThirdPartyNoticesViewModel : ViewModelBase
{
    private readonly IAboutInfoService _aboutInfoService;
    private readonly IThirdPartyNoticesService _thirdPartyNoticesService;

    public ThirdPartyNoticesViewModel(IServiceProvider serviceProvider, IAboutInfoService aboutInfoService,
        IThirdPartyNoticesService thirdPartyNoticesService)
        : base(serviceProvider)
    {
        _aboutInfoService = aboutInfoService;
        _thirdPartyNoticesService = thirdPartyNoticesService;

        ValidateUsingDataAnnotations = false;

        Title = LanguageHelper.GetRequiredString("Orchestra_ThirdPartyNotices_Title");
        Explanation = string.Empty;
        ThirdPartyNotices = new List<IThirdPartyNotice>();
    }

    public string Explanation { get; private set; }

    public IReadOnlyList<IThirdPartyNotice> ThirdPartyNotices { get; private set; }

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        var aboutInfo = await _aboutInfoService.GetAboutInfoAsync();
        var explanation = LanguageHelper.GetRequiredString("Orchestra_ThirdPartyNotices_Explanation");
        Explanation = string.Format(explanation, aboutInfo.Company, aboutInfo.ProductName);

        ThirdPartyNotices = await _thirdPartyNoticesService.GetThirdPartyNoticesAsync();
    }
}
