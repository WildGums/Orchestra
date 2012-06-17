namespace Orchestra.ViewModels
{
    using System.Reflection;
    using Catel.MVVM;
    using Catel.Reflection;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
            : base()
        {
        }
        #endregion

        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title { get { return Assembly.GetExecutingAssembly().Title(); } }
    }
}
