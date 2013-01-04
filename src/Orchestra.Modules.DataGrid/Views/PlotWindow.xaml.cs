namespace Orchestra.Modules.DataGrid.Views
{
    using Catel.Windows;

    using ViewModels;

    /// <summary>
    /// Interaction logic for PlotWindow.xaml.
    /// </summary>
    public partial class PlotWindow : DataWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlotWindow"/> class.
        /// </summary>
        public PlotWindow()
            : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public PlotWindow(PlotViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
