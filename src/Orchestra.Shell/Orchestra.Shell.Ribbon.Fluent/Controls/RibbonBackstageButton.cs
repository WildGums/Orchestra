// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonBackstageButton.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class RibbonBackstageButton : Button
    {
        public bool ShowBorder
        {
            get { return (bool)GetValue(ShowBorderProperty); }
            set { SetValue(ShowBorderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowBorder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowBorderProperty = DependencyProperty.Register("ShowBorder", typeof(bool), 
            typeof(RibbonBackstageButton), new PropertyMetadata(true));
    }
}