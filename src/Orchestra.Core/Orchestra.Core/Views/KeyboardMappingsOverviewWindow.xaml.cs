// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyboardMappingsOverviewWindow.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using Catel.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for KeyboardMappingsOverviewWindow.xaml.
    /// </summary>
    public partial class KeyboardMappingsOverviewWindow : DataWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardMappingsOverviewWindow"/> class.
        /// </summary>
        public KeyboardMappingsOverviewWindow()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardMappingsOverviewWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public KeyboardMappingsOverviewWindow(KeyboardMappingsOverviewViewModel viewModel)
            : base(viewModel, DataWindowMode.Custom)
        {
            AddCustomButton(new DataWindowButton("Print", "Print"));
            AddCustomButton(new DataWindowButton("Customize", "Customize"));
            AddCustomButton(new DataWindowButton("Close", Close));

            InitializeComponent();
        }
        #endregion
    }
}