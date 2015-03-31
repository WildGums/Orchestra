// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonExtensions.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM.Converters;
    using Services;

#if NET40
    using Microsoft.Windows.Controls.Ribbon;
#else
    using System.Windows.Controls.Ribbon;
#endif

    public static class RibbonExtensions
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static void AddAboutButton(this Ribbon ribbon)
        {
            Argument.IsNotNull(() => ribbon);

            Log.Debug("Adding about button to ribbon");

            var stackPanel = ribbon.EnsureHelpPaneStackPanel();

            var aboutButton = CreateRibbonButton(GetImageUri("/Resources/Images/about.png"));
            aboutButton.Click += (sender, e) =>
            {
                var aboutService = ServiceLocator.Default.ResolveType<IAboutService>();
                aboutService.ShowAbout();
            };
            stackPanel.Children.Add(aboutButton);
        }

        public static void AddMinimizeAndMaximizeButtons(this Ribbon ribbon)
        {
            Argument.IsNotNull(() => ribbon);

            Log.Debug("Adding minimize and maximize buttons to ribbon");

            var stackPanel = ribbon.EnsureHelpPaneStackPanel();

            var booleanToCollapsingVisibilityConverter = new BooleanToCollapsingVisibilityConverter();

            var minimizeButton = CreateRibbonButton(GetImageUri("/Resources/Images/minimize.png"));
            minimizeButton.SetBinding(RibbonButton.VisibilityProperty, new Binding("IsMinimized")
            {
                Source = ribbon,
                Converter = booleanToCollapsingVisibilityConverter,
                ConverterParameter = false
            });
            minimizeButton.Click += (sender, e) => ribbon.IsMinimized = true;
            stackPanel.Children.Add(minimizeButton);

            var maximizeButton = CreateRibbonButton(GetImageUri("/Resources/Images/maximize.png"));
            maximizeButton.SetBinding(RibbonButton.VisibilityProperty, new Binding("IsMinimized")
            {
                Source = ribbon,
                Converter = booleanToCollapsingVisibilityConverter,
                ConverterParameter = true
            });
            maximizeButton.Click += (sender, e) => ribbon.IsMinimized = false;
            stackPanel.Children.Add(maximizeButton);
        }

        private static StackPanel EnsureHelpPaneStackPanel(this Ribbon ribbon)
        {
            var helpPaneContent = ribbon.HelpPaneContent as StackPanel;
            if (helpPaneContent == null)
            {
                helpPaneContent = new StackPanel();
                helpPaneContent.Orientation = Orientation.Horizontal;

                ribbon.HelpPaneContent = helpPaneContent;
            }

            return helpPaneContent;
        }

        private static Uri GetImageUri(string uri)
        {
            var finalUri = string.Format("/{0};component{1}", typeof (RibbonExtensions).Assembly.GetName().Name, uri);
            return new Uri(finalUri, UriKind.RelativeOrAbsolute);
        }

        private static RibbonButton CreateRibbonButton(Uri smallImageSource)
        {
            var ribbonButton = new RibbonButton();

            ribbonButton.SmallImageSource = new BitmapImage(smallImageSource);

            return ribbonButton;
        }

    //<ribbon:Ribbon x:Name="ribbon">
    //    <ribbon:Ribbon.HelpPaneContent>
    //        <StackPanel Orientation="Horizontal">
    //            <ribbon:RibbonButton SmallImageSource="/Resources/Images/minimize.png" Click="ButtonCollapse_OnClick"
    //                    Visibility="{Binding ElementName=ribbon, Path=IsMinimized, Converter={StaticResource BooleanToCollapsingVisibilityConverter}, ConverterParameter=false}" />
    //            <ribbon:RibbonButton SmallImageSource="/Resources/Images/restore.png" Click="ButtonRestore_OnClick"
    //                    Visibility="{Binding ElementName=ribbon, Path=IsMinimized, Converter={StaticResource BooleanToCollapsingVisibilityConverter}}" />
    //            <ribbon:RibbonButton SmallImageSource="/Resources/Images/help.png" Command="{catel:CommandManagerBinding Help.About}" />
    //        </StackPanel>
    //    </ribbon:Ribbon.HelpPaneContent>
    }
}