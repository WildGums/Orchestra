// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotDemoModule.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.PlotDemo
{
    using Catel.MVVM;
    using Models;
    using Services;
    using ViewModels;

    /// <summary>
    /// Plot demo module.
    /// </summary>
    public class PlotDemoModule : ModuleBase
    {
        /// <summary>
        /// The module name.
        /// </summary>
        public const string Name = "PlotDemo";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PlotDemoModule()
            : base(Name)
        {
        }

        /// <summary>
        /// Called when the module has been initialized.
        /// </summary>
        protected override void OnInitialized()
        {
            var orchestraService = GetService<IOrchestraService>();
            orchestraService.ShowDocument<PlotDemoViewModel>();
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
            ribbonService.RegisterRibbonItem(new RibbonItem(HomeRibbonTabName, ModuleName, "Show", new Command(() => orchestraService.ShowDocument<PlotDemoViewModel>())));

            // View specific
            // TODO: Register view specific ribbon items
        }
    }
}