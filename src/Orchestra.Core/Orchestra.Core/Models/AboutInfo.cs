// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutInfo.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using System;
    using Catel;
    using Catel.Data;

    public class AboutInfo : ModelBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutInfo"/> class.
        /// </summary>
        /// <param name="logoImageSource">The logo image source.</param>
        /// <param name="url">The URL. Can be <c>null</c>.</param>
        /// <exception cref="ArgumentException">The <paramref name="logoImageSource"/> is <c>null</c> or whitespace.</exception>
        public AboutInfo(string logoImageSource, string url = null)
        {
            //Argument.IsNotNull(() => logoImageSource);

            LogoImageSource = logoImageSource;
            Url = url;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the logo image source.
        /// </summary>
        /// <value>The logo image source.</value>
        public string LogoImageSource { get; private set; }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; private set; }
        #endregion
    }
}