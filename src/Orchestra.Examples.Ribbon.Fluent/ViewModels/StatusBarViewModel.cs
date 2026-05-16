namespace Orchestra.Examples.Ribbon.ViewModels;

using System;
using Catel.MVVM;
using Catel.Services;

public partial class StatusBarViewModel : ViewModelBase
{
    private readonly ILanguageService _languageService;

    public StatusBarViewModel(IServiceProvider serviceProvider, ILanguageService languageService)
        : base(serviceProvider)
    {
        _languageService = languageService;
    }

    public override string Title
    {
        get { return _languageService.GetRequiredString("Orchestra_Examples_Ribbon_StatusBarViewModel_Title"); }
    }

    public bool EnableAutomaticUpdates { get; set; }
}
