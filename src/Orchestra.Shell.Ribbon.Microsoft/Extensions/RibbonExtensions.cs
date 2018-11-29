// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM.Converters;
    using Services;
    using System.Windows.Controls.Ribbon;

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
                aboutService.ShowAboutAsync();
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
            minimizeButton.SetBinding(UIElement.VisibilityProperty, new Binding("IsMinimized")
            {
                Source = ribbon,
                Converter = booleanToCollapsingVisibilityConverter,
                ConverterParameter = false
            });
            minimizeButton.Click += (sender, e) => ribbon.SetCurrentValue(Ribbon.IsMinimizedProperty, true);
            stackPanel.Children.Add(minimizeButton);

            var maximizeButton = CreateRibbonButton(GetImageUri("/Resources/Images/maximize.png"));
            maximizeButton.SetBinding(UIElement.VisibilityProperty, new Binding("IsMinimized")
            {
                Source = ribbon,
                Converter = booleanToCollapsingVisibilityConverter,
                ConverterParameter = true
            });
            maximizeButton.Click += (sender, e) => ribbon.SetCurrentValue(Ribbon.IsMinimizedProperty, false);
            stackPanel.Children.Add(maximizeButton);
        }

        private static StackPanel EnsureHelpPaneStackPanel(this Ribbon ribbon)
        {
            var helpPaneContent = ribbon.HelpPaneContent as StackPanel;
            if (helpPaneContent == null)
            {
                helpPaneContent = new StackPanel();
                helpPaneContent.SetCurrentValue(StackPanel.OrientationProperty, Orientation.Horizontal);

                ribbon.SetCurrentValue(Ribbon.HelpPaneContentProperty, helpPaneContent);
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
