// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemInfoWindow.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2015 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using System.Windows;
    using Catel.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for SystemInfoWindow.xaml.
    /// </summary>
    public partial class SystemInfoWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemInfoWindow"/> class.
        /// </summary>
        public SystemInfoWindow()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemInfoWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public SystemInfoWindow(SystemInfoViewModel viewModel)
            : base(viewModel, DataWindowMode.Custom)
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
    }
}