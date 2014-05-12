namespace Orchestra.Views
{
    using Catel.Windows;
    using Orchestra.ViewModels;

    /// <summary>
    /// Interaction logic for KeyboardMappingsOverviewWindow.xaml.
    /// </summary>
    public partial class KeyboardMappingsOverviewWindow : DataWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardMappingsOverviewWindow"/> class.
        /// </summary>
        public KeyboardMappingsOverviewWindow()
            : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardMappingsOverviewWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public KeyboardMappingsOverviewWindow(KeyboardMappingsOverviewViewModel viewModel)
            : base(viewModel, DataWindowMode.Custom)
        {
            AddCustomButton(new DataWindowButton("Customize", "Customize"));
            AddCustomButton(new DataWindowButton("Close", Close));

            InitializeComponent();
        }
    }
}
