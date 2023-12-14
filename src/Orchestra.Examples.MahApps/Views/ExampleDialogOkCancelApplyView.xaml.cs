namespace Orchestra.Examples.MahApps.Views
{
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for ExampleDialogOkCancelApplyView..xaml.
    /// </summary>
    public partial class ExampleDialogOkCancelApplyView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleDialogOkCancelApplyView"/> class.
        /// </summary>
        public ExampleDialogOkCancelApplyView()
            : base(DataWindowMode.OkCancelApply)
        {
            InitializeComponent();
        }
        #endregion
    }
}