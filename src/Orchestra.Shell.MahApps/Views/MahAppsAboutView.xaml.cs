namespace Orchestra.Views
{
    using Catel.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for MahAppsAboutView.xaml.
    /// </summary>
    public partial class MahAppsAboutView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MahAppsAboutView"/> class.
        /// </summary>
        public MahAppsAboutView()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MahAppsAboutView"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public MahAppsAboutView(AboutViewModel? viewModel)
            : base(viewModel, DataWindowMode.Close)
        {
            InitializeComponent();
        }
    }
}
