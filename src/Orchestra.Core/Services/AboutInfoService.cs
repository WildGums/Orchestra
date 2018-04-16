// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutInfoService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using Models;

    public class AboutInfoService : IAboutInfoService
    {
        #region IAboutInfoService Members
        public AboutInfo GetAboutInfo()
        {
            var aboutInfo = new AboutInfo(new Uri("pack://application:,,,/Resources/Images/CompanyLogo.png", UriKind.RelativeOrAbsolute));
            return aboutInfo;
        }
        #endregion
    }
}