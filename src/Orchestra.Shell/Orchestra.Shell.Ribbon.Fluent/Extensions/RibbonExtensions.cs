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
    using Fluent;
    using Services;
    using FluentButton = Fluent.Button;

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

        [Obsolete("Use AddMinimizeAndMaximizeButtons() instead", true)]
        public static void AddMinimizeMaximizeButton(this Ribbon ribbon)
        {
            AddMinimizeAndMaximizeButtons(ribbon);
        }

        public static void AddMinimizeAndMaximizeButtons(this Ribbon ribbon)
        {
            Argument.IsNotNull(() => ribbon);

            Log.Debug("Adding minimize and maximize buttons to ribbon");

            var stackPanel = ribbon.EnsureHelpPaneStackPanel();

            var booleanToCollapsingVisibilityConverter = new BooleanToCollapsingVisibilityConverter();

            var minimizeButton = CreateRibbonButton(GetImageUri("/Resources/Images/minimize.png"));
            minimizeButton.SetBinding(FluentButton.VisibilityProperty, new Binding("IsMinimized")
            {
                Source = ribbon,
                Converter = booleanToCollapsingVisibilityConverter,
                ConverterParameter = false
            });
            minimizeButton.Click += (sender, e) => ribbon.IsMinimized = true;
            stackPanel.Children.Add(minimizeButton);

            var maximizeButton = CreateRibbonButton(GetImageUri("/Resources/Images/maximize.png"));
            maximizeButton.SetBinding(FluentButton.VisibilityProperty, new Binding("IsMinimized")
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
            //var helpPaneContent = ribbon.HelpPaneContent as StackPanel;
            StackPanel helpPaneContent = null;
            if (helpPaneContent == null)
            {
                helpPaneContent = new StackPanel();
                helpPaneContent.Orientation = Orientation.Horizontal;

                //ribbon.HelpPaneContent = helpPaneContent;
            }

            return helpPaneContent;
        }

        private static Uri GetImageUri(string uri)
        {
            var finalUri = string.Format("/{0};component{1}", typeof (RibbonExtensions).Assembly.GetName().Name, uri);
            return new Uri(finalUri, UriKind.RelativeOrAbsolute);
        }

        private static FluentButton CreateRibbonButton(Uri smallImageSource)
        {
            var ribbonButton = new FluentButton();

            ribbonButton.Icon = new BitmapImage(smallImageSource);

            return ribbonButton;
        }
    }
}