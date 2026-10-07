namespace Orchestra.Examples.Ribbon.ViewModels;

using System;
using Catel.MVVM;
using Catel.Services;

public partial class FontSizeSelectorViewModel : ViewModelBase
{
    private readonly ILanguageService _languageService;

    public FontSizeSelectorViewModel(IServiceProvider serviceProvider, ILanguageService languageService)
        : base(serviceProvider)
    {
        _languageService = languageService;

        Title = _languageService.GetRequiredString("Orchestra_Examples_Ribbon_FontSizeSelectorViewModel_Title");
    }
}
