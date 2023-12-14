namespace Orchestra.Views
{
    using System.Windows.Automation.Peers;
    using Automation.Views;
    using Catel.Services;
    using Catel.Windows;
    using ViewModels;

    public partial class MessageBoxWindow
    {
        public MessageBoxWindow()
            : this(null)
        {
        }

        public MessageBoxWindow(MessageBoxViewModel? viewModel)
            : base(viewModel, DataWindowMode.Custom)
        {
            InitializeComponent();

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
