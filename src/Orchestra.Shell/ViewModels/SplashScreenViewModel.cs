// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SplashScreenViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using Catel.MVVM.ViewModels;

    /// <summary>
    /// A splash screen view model.
    /// </summary>
    public class SplashScreenViewModel : ProgressNotifyableViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SplashScreenViewModel"/> class.
        /// </summary>
        /// <remarks>Must have a public constructor in order to be serializable.</remarks>
        public SplashScreenViewModel()
            : base(false, false, false)
        {
        }
    }
}