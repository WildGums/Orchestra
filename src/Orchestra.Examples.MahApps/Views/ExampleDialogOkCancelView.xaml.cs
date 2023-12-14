namespace Orchestra.Examples.MahApps.Views
{
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for ExampleDialogOkCancelView.xaml.
    /// </summary>
    public partial class ExampleDialogOkCancelView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleDialogOkCancelView"/> class.
        /// </summary>
        public ExampleDialogOkCancelView()
            : base(DataWindowMode.OkCancel)
        {
            InitializeComponent();
        }
        #endregion
    }
}