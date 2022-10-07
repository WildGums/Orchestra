// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Windows.Media;
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
            ArgumentNullException.ThrowIfNull(ribbon);

            ribbon.AddRibbonButton(GetImageUri("/Resources/Images/about.png"), () =>
            {
                var aboutService = ServiceLocator.Default.ResolveType<IAboutService>();
                aboutService.ShowAboutAsync();
            });
        }

        public static Button AddRibbonButton(this Ribbon ribbon, ImageSource imageSource, Action action)
        {
            ArgumentNullException.ThrowIfNull(ribbon);

            var button = AddRibbonButton(ribbon, action);

            if (imageSource is not null)
            {
                button.SetCurrentValue(FluentButton.IconProperty, imageSource);
            }

            return button;
        }

        public static Button AddRibbonButton(this Ribbon ribbon, Uri imageUri, Action action)
        {
            ArgumentNullException.ThrowIfNull(ribbon);

            return AddRibbonButton(ribbon, new BitmapImage(imageUri), action);
        }

        private static Button AddRibbonButton(this Ribbon ribbon, Action action)
        {
            ArgumentNullException.ThrowIfNull(ribbon);

            Log.Debug("Adding button to ribbon");

            var ribbonButton = new Button();
            ribbonButton.Size = RibbonControlSize.Small;

            if (action is not null)
            {
                ribbonButton.Click += (sender, e) => action();
            }

            ribbon.ToolBarItems.Add(ribbonButton);

            return ribbonButton;
        }

        private static Uri GetImageUri(string uri)
        {
            var finalUri = string.Format("pack://application:,,,/{0};component{1}", typeof(RibbonExtensions).Assembly.GetName().Name, uri);
            return new Uri(finalUri, UriKind.RelativeOrAbsolute);
        }
    }
}