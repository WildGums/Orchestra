// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SplashScreen.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Views
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Media.Imaging;
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : DataWindow
    {
        private const string SplashScreenLocation = "Resources\\Images\\SplashScreen.png";
        private const string SplashScreenFallbackLocation = "/Orchestra.Shell;component/Resources/Images/SplashScreen.png";

        /// <summary>
        /// Initializes a new instance of the <see cref="SplashScreen" /> class.
        /// </summary>
        public SplashScreen()
            : base(DataWindowMode.Custom)
        {
            InitializeComponent();

            InitializeSplashScreen();
        }

        /// <summary>
        /// Initializes the splash screen.
        /// </summary>
        private void InitializeSplashScreen()
        {
            var directory = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

            try
            {
                string firstAttemptFile = Path.Combine(directory, SplashScreenLocation);
                if (File.Exists(firstAttemptFile))
                {
                    splashScreenImage.Source = new BitmapImage(new Uri(firstAttemptFile, UriKind.Absolute));
                    return;
                }
            }
            catch (Exception)
            {
                // Swallow exception
            }

            splashScreenImage.Source = new BitmapImage(new Uri(SplashScreenFallbackLocation, UriKind.RelativeOrAbsolute));
        }
    }
}