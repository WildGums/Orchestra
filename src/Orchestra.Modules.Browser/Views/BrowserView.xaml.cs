namespace Orchestra.Modules.Browser.Views
{
    using Catel.IoC;
    using Catel.Messaging;
    using Models;
    using Orchestra.Views;
    using ViewModels;

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

        /// <summary>
        /// Gets the view model that is contained by the container.
        /// </summary>
        /// <value>The view model.</value>
        public new BrowserViewModel ViewModel { get { return (BrowserViewModel)base.ViewModel; } }

        /// <summary>
        /// Initializes the ribbon.
        /// <para/>
        /// This is an ease-of-use method to register ribbons without having to care about any view model initialization. This
        /// method is only invoked once and when a view model is available.
        /// </summary>
        protected override void InitializeRibbon()
        {
            AddRibbonItem(new RibbonItem(BrowserModule.ModuleName, BrowserModule.ModuleName, "Back", ViewModel.GoBack) { ItemImage = "/Orchestra.Modules.Browser;component/Resources/Images/action_left.png"});
            AddRibbonItem(new RibbonItem(BrowserModule.ModuleName, BrowserModule.ModuleName, "Forward", ViewModel.GoForward) { ItemImage = "/Orchestra.Modules.Browser;component/Resources/Images/action_right.png" });
            AddRibbonItem(new RibbonItem(BrowserModule.ModuleName, BrowserModule.ModuleName, "Browse", ViewModel.Browse) { ItemImage = "/Orchestra.Modules.Browser;component/Resources/Images/action_browse.png" });
        }

        private void OnBrowse(string url)
        {
            webBrowser.Navigate(url);
        }
    }
}
