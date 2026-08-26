namespace Orchestra.Views;

using Catel.Services;
using Catel.Windows;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Interaction logic for KeyboardMappingsOverviewWindow.xaml.
/// </summary>
public partial class KeyboardMappingsOverviewWindow
{
    partial void OnInitializingComponent()
    {
        Mode = DataWindowMode.Custom;

        var languageService = ServiceProvider.GetRequiredService<ILanguageService>();

        AddCustomButton(new DataWindowButton(languageService.GetRequiredString("Orchestra_Customize"), "Customize"));
        AddCustomButton(DataWindowButton.FromSync(ServiceProvider, languageService.GetRequiredString("Orchestra_Close"), Close, null));
    }
}
