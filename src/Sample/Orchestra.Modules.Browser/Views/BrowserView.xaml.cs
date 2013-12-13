// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrowserView.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Modules.Browser.Views
{
    using System.Windows.Navigation;
    using Catel.IoC;
    using Catel.Messaging;
    using Orchestra.Modules.Browser.ViewModels;
    using Orchestra.Views;

    /// <summary>
    /// Interaction logic for BrowserView.xaml.
    /// </summary>
    public partial class BrowserView : DocumentView
    {
        #region Constants
        private const string UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 7.1; Trident/5.0)";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserView"/> class.
        /// </summary>
        public BrowserView()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Called when the view model has changed.
        /// </summary>
        protected override void OnViewModelChanged()
        {
            var vm = ViewModel as BrowserViewModel;
            if (vm != null)
            {
                if (!string.IsNullOrWhiteSpace(vm.Url))
                {
                    OnBrowse(vm.Url);
                }

                var messageMediator = ServiceLocator.Default.ResolveType<IMessageMediator>();
                messageMediator.Register<string>(this, OnBrowse, vm.UrlChangedMessageTag);
            }
        }

        private void OnBrowse(string url)
        {
            webBrowser.Navigate(url, null, null, string.Format("User-Agent: {0}", UserAgent));
        }
        #endregion
    }
}