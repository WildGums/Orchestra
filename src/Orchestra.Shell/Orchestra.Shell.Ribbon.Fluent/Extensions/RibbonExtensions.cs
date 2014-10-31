// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonExtensions.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Windows.Media.Imaging;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
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

            var aboutButton = CreateRibbonButton(GetImageUri("/Resources/Images/about.png"));
            aboutButton.Click += (sender, e) =>
            {
                var aboutService = ServiceLocator.Default.ResolveType<IAboutService>();
                aboutService.ShowAbout();
            };

            var toolbarItems = ribbon.ToolBarItems;
            toolbarItems.Add(aboutButton);
        }

        private static Uri GetImageUri(string uri)
        {
            var finalUri = string.Format("pack://application:,,,/{0};component{1}", typeof(RibbonExtensions).Assembly.GetName().Name, uri);
            return new Uri(finalUri, UriKind.RelativeOrAbsolute);
        }

        private static FluentButton CreateRibbonButton(Uri smallImageSource)
        {
            var ribbonButton = new FluentButton();

            ribbonButton.Size = RibbonControlSize.Small;
            ribbonButton.Icon = new BitmapImage(smallImageSource);

            return ribbonButton;
        }
    }
}