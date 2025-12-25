namespace Orchestra.Views
{
    using System.Windows.Automation.Peers;
    using Automation.Views;
    using Catel.Services;
    using ViewModels;

    public partial class MessageBoxWindow
    {
        partial void OnInitializedComponent()
        {
            var viewModel = ViewModel as MessageBoxViewModel;
            if (viewModel?.Button == MessageButton.YesNo)
            {
                this.DisableCloseButton();
            }
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new MessageBoxWindowAutomationPeer(this);
        }
    }
}
