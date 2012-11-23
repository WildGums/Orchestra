// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrowserModule.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.Browser
{
    using Catel.MVVM;
    using Models;
    using Services;
    using ViewModels;

    /// <summary>
    /// Browser module.
    /// </summary>
    public class BrowserModule : ModuleBase
    {
        /// <summary>
        /// The module name.
        /// </summary>
        public const string Name = "Browser";

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserModule"/> class. 
        /// </summary>
        public BrowserModule()
            : base(Name)
        {
        }

        /// <summary>
        /// Called when the module has been initialized.
        /// </summary>
        protected override void OnInitialized()
        {
            var orchestraService = GetService<IOrchestraService>();

            var openRibbonItem = new RibbonItem(ModuleName, ModuleName, "Open", new Command(() => orchestraService.ShowDocument<BrowserViewModel>()));
            orchestraService.AddRibbonItem(openRibbonItem);

            orchestraService.ShowDocument<BrowserViewModel>();
        }
    }
}