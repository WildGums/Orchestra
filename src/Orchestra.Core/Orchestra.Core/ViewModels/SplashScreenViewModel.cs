// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SplashScreenViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Reflection;

    /// <summary>
    /// The splash screen view model.
    /// </summary>
    public class SplashScreenViewModel : ViewModelBase
    {
        #region Properties
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
        protected override async Task Initialize()
        {
            await base.Initialize();

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