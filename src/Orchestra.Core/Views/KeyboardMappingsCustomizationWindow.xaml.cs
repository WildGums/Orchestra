namespace Orchestra.Views;

using Catel.Windows;
using Orchestra.ViewModels;

/// <summary>
/// Interaction logic for KeyboardMappingsWindow.xaml.
/// </summary>
public partial class KeyboardMappingsCustomizationWindow
{
    partial void OnInitializingComponent()
    {
        Mode = DataWindowMode.Close;
    }
}
