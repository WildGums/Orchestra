namespace Orchestra.Views
{
    using System.Windows;
    using Catel.Windows;
    using ViewModels;
    using Windows;

    public partial class AboutWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutWindow"/> class.
        /// </summary>
        public AboutWindow()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public AboutWindow(AboutViewModel? viewModel)
            : base(viewModel, DataWindowMode.Custom)
        {
            InitializeComponent();

            this.ApplyApplicationIcon();
        }

        private void Close_OnClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
