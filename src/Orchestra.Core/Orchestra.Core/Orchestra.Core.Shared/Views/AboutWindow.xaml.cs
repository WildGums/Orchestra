// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutWindow.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using System.Windows;
    using Catel.IoC;
    using Catel.Services;
    using Catel.Windows;
    using ViewModels;
    using Windows;

    /// <summary>
    /// Interaction logic for AboutWindow.xaml.
    /// </summary>
    public partial class AboutWindow : DataWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutWindow"/> class.
        /// </summary>
        public AboutWindow()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public AboutWindow(AboutViewModel viewModel)
            : base(viewModel, DataWindowMode.Custom)
        {
            InitializeComponent();

            this.ApplyApplicationIcon();
        }
        #endregion

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}