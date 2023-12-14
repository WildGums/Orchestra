namespace Orchestra.Examples.MahApps.Views
{
    public sealed partial class ExampleDataWindow
    {
        #region Constructors
        public ExampleDataWindow()
            //: base(DataWindowMode.Custom)
        {
            //AddCustomButton(new DataWindowButton("Save anyway", async () => await ExecuteOkAsync(), OnOkCanExecute));
            //AddCustomButton(new DataWindowButton("Cancel", async () => await ExecuteCancelAsync(), OnCancelCanExecute));

            InitializeComponent();
        }
        #endregion
    }
}