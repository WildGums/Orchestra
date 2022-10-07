// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlyoutInfo.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using Catel;
    using MahApps.Metro.Controls;

    public class FlyoutInfo
    {
        #region Constructors
        public FlyoutInfo(Flyout flyout, object content)
        {
            ArgumentNullException.ThrowIfNull(flyout);
            ArgumentNullException.ThrowIfNull(content);

            Flyout = flyout;
            Content = content;
        }
        #endregion

        public Flyout Flyout { get; set; }
        public object Content { get; set; }
    }
}