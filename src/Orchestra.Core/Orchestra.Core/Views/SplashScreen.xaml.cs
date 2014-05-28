// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SplashScreen.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using System.Windows;
    using System.Windows.Media;
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : DataWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SplashScreen" /> class.
        /// </summary>
        public SplashScreen()
            : base(DataWindowMode.Custom)
        {
            InitializeComponent();

            Background = Brushes.DodgerBlue;

            var application = Application.Current;
            if (application != null)
            {
                Background = ThemeHelper.GetAccentColorBrush();
            }
        }
        #endregion 
    }
}