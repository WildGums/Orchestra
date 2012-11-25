// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridView.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.DataGrid.Views
{
    using Orchestra.Models;
    using Orchestra.Modules.DataGrid.ViewModels;

    /// <summary>
    /// Interaction logic for DataGridView.xaml
    /// </summary>
    public partial class DataGridView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridView"/> class.
        /// </summary>
        public DataGridView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the view model that is contained by the container.
        /// </summary>
        /// <value>The view model.</value>
        public new DataGridViewModel ViewModel
        {
            get { return (DataGridViewModel)base.ViewModel; }
        }

        /// <summary>
        /// Initializes the ribbon.
        /// <para />
        /// This is an ease-of-use method to register ribbons without having to care about any view model initialization. This
        /// method is only invoked once and when a view model is available.
        /// </summary>
        protected override void InitializeRibbon()
        {
            AddRibbonItem(new RibbonItem(DataGridModule.Name, DataGridModule.Name, "Open file", ViewModel.OpenFileCommand));
            AddRibbonItem(new RibbonItem(DataGridModule.Name, DataGridModule.Name, "Save file", ViewModel.SaveToFileCommand));
        }
    }
}