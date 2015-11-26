// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SplashScreenViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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
        private readonly ILanguageService _languageService;

        public SplashScreenViewModel(IAboutInfoService aboutInfoService, ILanguageService languageService)
        {
            Argument.IsNotNull(() => aboutInfoService);
            Argument.IsNotNull(() => languageService);

            _languageService = languageService;

            var aboutInfo = aboutInfoService.GetAboutInfo();
            CompanyLogoForSplashScreenUri = aboutInfo.CompanyLogoForSplashScreenUri;
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

            var assembly = Orchestra.AssemblyHelper.GetEntryAssembly();
            if (assembly != null)
            {
                Title = assembly.Title();
                Company = assembly.Company();
                ProducedBy = string.Format(_languageService.GetString("Orchestra_ProducedBy"), Company);
                Version = VersionHelper.GetCurrentVersion(assembly);
            }
        }
        #endregion
    }
}