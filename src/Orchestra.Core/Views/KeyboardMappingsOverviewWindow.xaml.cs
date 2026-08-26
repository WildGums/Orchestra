namespace Orchestra.Views;

using Catel.IoC;
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

        var serviceProvider = IoCContainer.ServiceProvider;
        var languageService = serviceProvider.GetRequiredService<ILanguageService>();

        AddCustomButton(new DataWindowButton(languageService.GetRequiredString("Orchestra_Customize"), "Customize"));
        AddCustomButton(DataWindowButton.FromSync(serviceProvider, languageService.GetRequiredString("Orchestra_Close"), Close, null));
    }
}
