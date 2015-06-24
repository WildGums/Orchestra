namespace Orchestra.Views
{
    using System.Linq;
    using Catel.Windows;
    using ViewModels;

    public sealed partial class MessageBox
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