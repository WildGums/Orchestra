// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonBackstageTabItem.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class RibbonBackstageTabItem : TabItem
    {
        public RibbonBackstageTabItem()
        {
        }

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource),
            typeof(RibbonBackstageTabItem), new PropertyMetadata(null, (sender, e) => ((RibbonBackstageTabItem)sender).BuildHeader()));

        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string),
            typeof(RibbonBackstageTabItem), new PropertyMetadata(string.Empty, (sender, e) => ((RibbonBackstageTabItem)sender).BuildHeader()));

        private void BuildHeader()
        {
            var header = new RibbonBackstageTabItemHeader
            {
                KeepIconSizeWithoutIcon = true,
                Icon = Icon,
                HeaderText = HeaderText
            };

            Header = header;
        }
    }
}