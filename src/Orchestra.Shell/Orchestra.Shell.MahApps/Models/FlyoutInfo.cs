// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlyoutInfo.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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
            Argument.IsNotNull(() => flyout);
            Argument.IsNotNull(() => content);

            Flyout = flyout;
            Content = content;
        }
        #endregion

        public Flyout Flyout { get; set; }
        public object Content { get; set; }
    }
}