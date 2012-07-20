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

            var showRibbonItem = new RibbonItem(ModuleName, ModuleName, "Show", new Command(() => orchestraService.ShowDocument<PlotDemoViewModel>()));
            orchestraService.AddRibbonItem(showRibbonItem);

            orchestraService.ShowDocument<PlotDemoViewModel>();
        }
    }
}