namespace Orchestra.Views
{
    using Catel.IoC;
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for KeyboardMappingsOverviewWindow.xaml.
    /// </summary>
    public partial class KeyboardMappingsOverviewWindow
    {
        partial void OnInitializedComponent()
        {
            Mode = DataWindowMode.Custom;

            AddCustomButton(new DataWindowButton("Customize", "Customize"));
            AddCustomButton(DataWindowButton.FromSync(IoCContainer.ServiceProvider, "Close", Close, null));
        }
    }
}
