namespace Orchestra.Modules.Browser.Views
{
    using Catel.IoC;
    using Catel.Messaging;
    using Orchestra.Views;

    /// <summary>
    /// Interaction logic for BrowserView.xaml.
    /// </summary>
    public partial class BrowserView : DocumentView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserView"/> class.
        /// </summary>
        public BrowserView()
        {
            InitializeComponent();

            var messageMediator = ServiceLocator.Instance.ResolveType<IMessageMediator>();
            messageMediator.Register<string>(this, OnBrowse, BrowserModule.ModuleName);
        }

        private void OnBrowse(string url)
        {
            webBrowser.Navigate(url);
        }
    }
}
