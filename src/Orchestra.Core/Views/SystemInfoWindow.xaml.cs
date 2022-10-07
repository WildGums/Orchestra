namespace Orchestra.Views
{
    using System.Windows;
    using Catel.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for SystemInfoWindow.xaml.
    /// </summary>
    public partial class SystemInfoWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemInfoWindow"/> class.
        /// </summary>
        public SystemInfoWindow()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemInfoWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public SystemInfoWindow(SystemInfoViewModel viewModel)
            : base(viewModel, DataWindowMode.Custom)
        {
            InitializeComponent();
        }

        private void OnCloseClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
