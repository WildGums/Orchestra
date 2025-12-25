namespace Orchestra.ChangeLogger.LogViews
{
    using Catel;
    using Catel.IoC;
    using Catel.Windows;

    public partial class ChangelogWindow
    {
        partial void OnInitializedComponent()
        { 
            Mode = DataWindowMode.Custom;

            var serviceProvider = IoCContainer.ServiceProvider;

            AddCustomButton(DataWindowButton.FromAsync(serviceProvider, LanguageHelper.GetRequiredString("OK"), OnOkExecuteAsync, OnOkCanExecute));
            AddCustomButton(DataWindowButton.FromAsync(serviceProvider, LanguageHelper.GetRequiredString("Orchestra_ChangelogRemindMeLater"), OnCancelExecuteAsync, OnCancelCanExecute));
        }
    }
}
