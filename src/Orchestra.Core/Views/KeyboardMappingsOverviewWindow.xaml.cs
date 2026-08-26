namespace Orchestra.Views;

using Catel;
using Catel.IoC;
using Catel.Windows;

/// <summary>
/// Interaction logic for KeyboardMappingsOverviewWindow.xaml.
/// </summary>
public partial class KeyboardMappingsOverviewWindow
{
    partial void OnInitializingComponent()
    {
        Mode = DataWindowMode.Custom;

        AddCustomButton(new DataWindowButton(LanguageHelper.GetRequiredString("Orchestra_Customize"), "Customize"));
        AddCustomButton(DataWindowButton.FromSync(IoCContainer.ServiceProvider, LanguageHelper.GetRequiredString("Orchestra_Close"), Close, null));
    }
}
