// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutInfo.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using System;
    using System.Reflection;
    using Catel.Data;

    public class AboutInfo : ModelBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutInfo" /> class.
        /// </summary>
        /// <param name="companyLogoForSplashScreenUri"></param>
        /// <param name="companyLogoUri">The company logo image Uri.</param>
        /// <param name="logoImageSource">The logo image source.</param>
        /// <param name="uriInfo">The uri info. Can be <c>null</c>.</param>
        /// <param name="assembly">The assembly to use for the information. If <c>null</c>, the assembly will be determined automatically.</param>
        public AboutInfo(Uri companyLogoUri = null, string logoImageSource = null, UriInfo uriInfo = null, Assembly assembly = null, Uri companyLogoForSplashScreenUri = null)
        {
            ShowLogButton = true;

            CompanyLogoForSplashScreenUri = companyLogoForSplashScreenUri;
            CompanyLogoUri = companyLogoUri;
            LogoImageSource = logoImageSource;
            UriInfo = uriInfo;
            Assembly = assembly ?? AssemblyHelper.GetEntryAssembly();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the logo image source.
        /// </summary>
        /// <value>The company logo image Uri.</value>
        public Uri CompanyLogoForSplashScreenUri { get; set; }

        /// <summary>
        /// Gets the logo image source.
        /// </summary>
        /// <value>The company logo image Uri.</value>
        public Uri CompanyLogoUri { get; private set; }

        /// <summary>
        /// Gets the logo image source.
        /// </summary>
        /// <value>The logo image source.</value>
        public string LogoImageSource { get; private set; }

        /// <summary>
        /// Gets the uri info.
        /// </summary>
        /// <value>The uri info.</value>
        public UriInfo UriInfo { get; private set; }

        /// <summary>
        /// Gets the assembly.
        /// </summary>
        /// <value>The assembly.</value>
        public Assembly Assembly { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the about should show a log button.
        /// </summary>
        /// <value><c>true</c> if the log button should be shown; otherwise, <c>false</c>.</value>
        public bool ShowLogButton { get; set; }
        #endregion
    }
}