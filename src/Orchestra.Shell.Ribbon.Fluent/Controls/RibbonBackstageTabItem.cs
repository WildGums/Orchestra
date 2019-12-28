// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonBackstageTabItem.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(ImageSource),
            typeof(RibbonBackstageTabItem), new PropertyMetadata(null, (sender, e) => ((RibbonBackstageTabItem)sender).BuildHeader()));


        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(nameof(HeaderText), typeof(string),
            typeof(RibbonBackstageTabItem), new PropertyMetadata(string.Empty, (sender, e) => ((RibbonBackstageTabItem)sender).BuildHeader()));

        private void BuildHeader()
        {
            var header = new RibbonBackstageTabItemHeader
            {
                KeepIconSizeWithoutIcon = true,
                Icon = Icon,
                HeaderText = HeaderText
            };

            SetCurrentValue(HeaderProperty, header);
        }
    }
}
