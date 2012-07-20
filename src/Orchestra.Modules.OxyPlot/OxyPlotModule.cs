// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyPlotModule.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.OxyPlot
{
    using Catel.MVVM;
    using Models;
    using Services;
    using ViewModels;

    /// <summary>
    /// The oxyplot module.
    /// </summary>
    public class OxyPlotModule : ModuleBase
    {
        /// <summary>
        /// The module name.
        /// </summary>
        public const string Name = "OxyPlot";

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Catel.Modules.ModuleBase`1"/> class.
        /// </summary>
        public OxyPlotModule()
            : base(Name)
        {
        }
        #endregion

        /// <summary>
        /// Called when the module has been initialized.
        /// </summary>
        protected override void OnInitialized()
        {
            var orchestraService = GetService<IOrchestraService>();

            var showRibbonItem = new RibbonItem(ModuleName, ModuleName, "Show", new Command(() => orchestraService.ShowDocument<OxyPlotViewModel>()));
            orchestraService.AddRibbonItem(showRibbonItem);

            orchestraService.ShowDocument<OxyPlotViewModel>();
        }
    }
}