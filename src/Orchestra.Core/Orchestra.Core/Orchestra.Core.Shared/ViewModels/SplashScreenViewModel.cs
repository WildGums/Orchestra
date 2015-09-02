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
    using Models;
    using Services;

    /// <summary>
    /// The splash screen view model.
    /// </summary>
    public class SplashScreenViewModel : ViewModelBase
    {
        public SplashScreenViewModel(IAboutInfoService aboutInfoService)
        {
            Argument.IsNotNull(() => aboutInfoService);

            var aboutInfo = aboutInfoService.GetAboutInfo();
            CompanyLogoForSplashScreenUri = aboutInfo.CompanyLogoForSplashScreenUri;
        }
        #region Properties
        public Uri CompanyLogoForSplashScreenUri { get; private set; }

        /// <summary>
        /// Gets the company.
        /// </summary>
        /// <value>The company.</value>
        public string Company { get; private set; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
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
                Version = VersionHelper.GetCurrentVersion(assembly);
            }
        }
        #endregion
    }
}