// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrowserViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.Browser.ViewModels
{
    using System.Collections.Generic;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Messaging;

    /// <summary>
    /// UserControl view model.
    /// </summary>
    public class BrowserViewModel : Orchestra.ViewModels.ViewModelBase
    {
        private readonly List<string> _previousPages = new List<string>();
        private readonly List<string> _nextPages = new List<string>();
        private readonly string _title;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserViewModel"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public BrowserViewModel(string title)
            : this()
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                _title = title;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserViewModel"/> class.
        /// </summary>
        public BrowserViewModel()
        {
            GoBack = new Command(OnGoBackExecute, OnGoBackCanExecute);
            GoForward = new Command(OnGoForwardExecute, OnGoForwardCanExecute);
            Browse = new Command(OnBrowseExecute, OnBrowseCanExecute);

            _title = "Browser";
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets the recent sites.
        /// </summary>
        /// <value>
        /// The recent sites.
        /// </value>
        public string[] RecentSites { get { return new[] {"Orchestra", "Catel"}; } }

        #region SelectedSite property

        /// <summary>
        /// Gets or sets the SelectedSite value.
        /// </summary>
        public string SelectedSite
        {
            get { return GetValue<string>(SelectedSiteProperty); }
            set { SetValue(SelectedSiteProperty, value); }
        }

        /// <summary>
        /// SelectedSite property data.
        /// </summary>
        public static readonly PropertyData SelectedSiteProperty = RegisterProperty("SelectedSite", typeof (string), null, OnSelectedSiteChanged);

        /// <summary>
        /// Called when the SelectedSite value changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="AdvancedPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnSelectedSiteChanged(object sender, AdvancedPropertyChangedEventArgs e)
        {
            var _this = ((BrowserViewModel) sender);

            switch (_this.SelectedSite)
            {
                case "Orchestra":
                    _this.Url = "http://www.github.com/Orcomp/Orchestra";
                    break;

                case "Catel":
                    _this.Url = "http://www.catelproject.com";
                    break;

                default:
                    return;
            }

            _this.OnBrowseExecute();
        }
        #endregion

        #endregion

        #region Commands
        /// <summary>
        /// Gets the GoBack command.
        /// </summary>
        public Command GoBack { get; private set; }

        /// <summary>
        /// Method to check whether the GoBack command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnGoBackCanExecute()
        {
            return _previousPages.Count > 0;
        }

        /// <summary>
        /// Method to invoke when the GoBack command is executed.
        /// </summary>
        private void OnGoBackExecute()
        {
            // TODO: Handle command logic here
        }

        /// <summary>
        /// Gets the GoForward command.
        /// </summary>
        public Command GoForward { get; private set; }

        /// <summary>
        /// Method to check whether the GoForward command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnGoForwardCanExecute()
        {
            return _nextPages.Count > 0;
        }

        /// <summary>
        /// Method to invoke when the GoForward command is executed.
        /// </summary>
        private void OnGoForwardExecute()
        {
            // TODO: Handle command logic here
        }

        /// <summary>
        /// Gets the Browse command.
        /// </summary>
        public Command Browse { get; private set; }

        /// <summary>
        /// Method to check whether the Browse command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnBrowseCanExecute()
        {
            return !string.IsNullOrWhiteSpace(Url);
        }

        /// <summary>
        /// Method to invoke when the Browse command is executed.
        /// </summary>
        private void OnBrowseExecute()
        {
            var url = Url;
            if (!url.StartsWith("http://"))
            {
                url = "http://" + url;
            }

            var messageMediator = GetService<IMessageMediator>();
            messageMediator.SendMessage(url, BrowserModule.Name);
        }
        #endregion
    }
}