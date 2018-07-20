// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutInfo.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using System;
    using System.Reflection;
    using Catel.Data;
    using Catel.Reflection;
    using System.Windows.Media.Imaging;

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
        /// <param name="appIcon">The application icon. Can be <c>null</c>. If <c>null</c> then value will be picked from assembly.</param>
        /// <param name="buildDateTime">The application build datetime. Can be <c>null</c>. If <c>null</c> then value will be picked from assembly.</param>
        /// <param name="company">The application company. Can be <c>null</c>. If <c>null</c> then value will be picked from assembly.</param>
        /// <param name="copyright">The application copyright. Can be <c>null</c>. If <c>null</c> then value will be picked from assembly.</param>
        /// <param name="copyrightUri">The application copyright Uri. Can be <c>null</c>.</param>
        /// <param name="description">The application description. Can be <c>null</c>. If <c>null</c> then value will be picked from assembly.</param>
        /// <param name="displayVersion">The application display version. Can be <c>null</c>. If <c>null</c> then value will be picked from assembly.</param>
        /// <param name="informationalVersion">The application informational version. Can be <c>null</c>. If <c>null</c> then value will be picked from assembly.</param>
        /// <param name="productName">The application product name. Can be <c>null</c>. If <c>null</c> then value will be picked from assembly.</param>
        /// <param name="name">The application title. Can be <c>null</c>. If <c>null</c> then value will be picked from assembly.</param>
        /// <param name="version">The application version. Can be <c>null</c>. If <c>null</c> then value will be picked from assembly.</param>
        public AboutInfo(Uri companyLogoUri = null, string logoImageSource = null, UriInfo uriInfo = null, Assembly assembly = null,
            Uri companyLogoForSplashScreenUri = null, BitmapSource appIcon = null, DateTime? buildDateTime = null, string company = null,
            string copyright = null, Uri copyrightUri = null, string description = null, string displayVersion = null, string informationalVersion = null,
            string name = null, string productName = null, string version = null)
        {
            ShowLogButton = true;

            Assembly = assembly ?? AssemblyHelper.GetEntryAssembly();

            CompanyLogoForSplashScreenUri = companyLogoForSplashScreenUri;
            CompanyLogoUri = companyLogoUri;
            CopyrightUri = copyrightUri;
            LogoImageSource = logoImageSource;
            UriInfo = uriInfo;

            AppIcon = appIcon ?? Assembly.ExtractLargestIcon();
            BuildDateTime = buildDateTime ?? Assembly.GetBuildDateTime();
            Company = company ?? Assembly.Company();
            Copyright = copyright ?? Assembly.Copyright();
            Description = description ?? Assembly.Description();
            InformationalVersion = informationalVersion ?? Assembly.InformationalVersion();
            ProductName = productName ?? Assembly.Product();
            Name = name ?? Assembly.Title();
            Version = version ?? Assembly.Version();

            DisplayVersion = displayVersion ?? VersionHelper.GetCurrentVersion(Assembly);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the assembly.
        /// </summary>
        /// <value>The assembly.</value>
        public Assembly Assembly { get; private set; }

        /// <summary>
        /// Gets the application icon.
        /// </summary>
        public BitmapSource AppIcon { get; private set; }

        /// <summary>
        /// Gets the application build datetime.
        /// </summary>
        public DateTime? BuildDateTime { get; private set; }

        /// <summary>
        /// Gets the application logo image Uri.
        /// </summary>
        /// <value>The company logo image Uri.</value>
        public Uri CompanyLogoForSplashScreenUri { get; set; }

        /// <summary>
        /// Gets the application logo image source.
        /// </summary>
        /// <value>The company logo image Uri.</value>
        public Uri CompanyLogoUri { get; private set; }

        /// <summary>
        /// Gets the application company.
        /// </summary>
        public string Company { get; private set; }

        /// <summary>
        /// Gets the application copyright.
        /// </summary>
        public string Copyright { get; private set; }

        /// <summary>
        /// Gets the application copyright Uri.
        /// </summary>
        public Uri CopyrightUri { get; private set; }

        /// <summary>
        /// Gets the application description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the application display version.
        /// </summary>
        public string DisplayVersion { get; private set; }

        /// <summary>
        /// Gets the application informational version.
        /// </summary>
        public string InformationalVersion { get; private set; }

        /// <summary>
        /// Gets the application logo image source.
        /// </summary>
        /// <value>The logo image source.</value>
        public string LogoImageSource { get; private set; }

        /// <summary>
        /// Gets the application name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the application product name.
        /// </summary>
        public string ProductName { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the about should show a log button.
        /// </summary>
        /// <value><c>true</c> if the log button should be shown; otherwise, <c>false</c>.</value>
        public bool ShowLogButton { get; set; }

        /// <summary>
        /// Gets the application uri info.
        /// </summary>
        /// <value>The uri info.</value>
        public UriInfo UriInfo { get; private set; }

        /// <summary>
        /// Gets the application version.
        /// </summary>
        public string Version { get; private set; }
        #endregion
    }
}