// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrowserView.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.Browser.Views
{
    using Catel;
    using Catel.IoC;
    using Catel.Messaging;
    using Orchestra.Views;
    using ViewModels;

    /// <summary>
    /// Interaction logic for BrowserView.xaml.
    /// </summary>
    public partial class BrowserView : DocumentView
    {
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
            webBrowser.Navigate(url);
        }
        #endregion
    }
}