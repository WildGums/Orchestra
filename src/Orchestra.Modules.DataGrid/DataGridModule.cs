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

            var open = new RibbonItem(ModuleName, ModuleName, "Open", new Command(() => orchestraService.ShowDocument<DataGridViewModel>()));
            orchestraService.AddRibbonItem(open);

            orchestraService.ShowDocument<DataGridViewModel>();
        }
    }
}