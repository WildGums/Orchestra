namespace Orchestra.Views
{
    using Catel.Windows;
    using Orchestra.ViewModels;

    /// <summary>
    /// Interaction logic for KeyboardMappingsWindow.xaml.
    /// </summary>
    public partial class KeyboardMappingsCustomizationWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardMappingsCustomizationWindow"/> class.
        /// </summary>
        public KeyboardMappingsCustomizationWindow()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardMappingsCustomizationWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public KeyboardMappingsCustomizationWindow(KeyboardMappingsCustomizationViewModel? viewModel)
            : base(viewModel, DataWindowMode.Close)
        {
            InitializeComponent();
        }
    }
}
