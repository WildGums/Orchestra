// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SplashScreenViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Models;
    using Services;

    /// <summary>
    /// The splash screen view model.
    /// </summary>
    public class SplashScreenViewModel : ViewModelBase
    {
        private readonly IAboutInfoService _aboutInfoService;
        private readonly ILanguageService _languageService;

        public SplashScreenViewModel(IAboutInfoService aboutInfoService, ILanguageService languageService)
        {
            Argument.IsNotNull(() => aboutInfoService);
            Argument.IsNotNull(() => languageService);

            _aboutInfoService = aboutInfoService;
            _languageService = languageService;
        }

        #region Properties
        public Uri CompanyLogoForSplashScreenUri { get; private set; }

        public string Company { get; private set; }

        public string ProducedBy { get; private set; }

        public string Version { get; private set; }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            var aboutInfo = _aboutInfoService.GetAboutInfo();

            Title = aboutInfo.Name;
            Company = aboutInfo.Company;
            CompanyLogoForSplashScreenUri = aboutInfo.CompanyLogoForSplashScreenUri;
            ProducedBy = string.Format(_languageService.GetString("Orchestra_ProducedBy"), Company);
            Version = aboutInfo.DisplayVersion;
        }
        #endregion
    }
}