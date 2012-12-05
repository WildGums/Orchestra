// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridModule.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.DataGrid
{
    using Catel.MVVM;

    using Orchestra.Models;
    using Orchestra.Modules.DataGrid.ViewModels;
    using Orchestra.Services;
    using Views;

    /// <summary>
    /// The data grid module.
    /// </summary>
    public class DataGridModule : ModuleBase
    {
        /// <summary>
        /// The module name.
        /// </summary>
        public const string Name = "DataGrid";

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridModule" /> class.
        /// </summary>
        public DataGridModule()
            : base(Name)
        {
        }

        /// <summary>
        /// The on initialized.
        /// </summary>
        protected override void OnInitialized()
        {
            var orchestraService = GetService<IOrchestraService>();
            orchestraService.ShowDocument<DataGridViewModel>();
        }

        /// <summary>
        /// Initializes the ribbon.
        /// <para />
        /// Use this method to hook up views to ribbon items.
        /// </summary>
        /// <param name="ribbonService">The ribbon service.</param>
        protected override void InitializeRibbon(IRibbonService ribbonService)
        {
            var orchestraService = GetService<IOrchestraService>();

            // Module specific
            ribbonService.RegisterRibbonItem(new RibbonItem(HomeRibbonTabName, ModuleName, "Open", new Command(() => orchestraService.ShowDocument<DataGridViewModel>())));

            // View specific
            ribbonService.RegisterViewSpecificRibbonItem<DataGridView>(new RibbonItem(Name, Name, "Open file", "OpenFileCommand"));
            ribbonService.RegisterViewSpecificRibbonItem<DataGridView>(new RibbonItem(Name, Name, "Save file", "SaveToFileCommand"));
        }
    }
}