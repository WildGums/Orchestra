namespace Orchestra.Views
{
    using Catel.Windows;
    using ViewModels;

    public partial class MessageBox
    {
        public MessageBox()
            : this(null)
        {
        }

        public MessageBox(MessageBoxViewModel viewModel)
            : base(viewModel, DataWindowMode.Custom)
        {
            this.InitializeComponent();
        }
    }
}