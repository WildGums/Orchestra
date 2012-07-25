// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyPlotModule.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.OxyPlot
{
    using Catel.IoC;
    using Catel.MVVM;
    using Models;
    using Orchestra.Services;
    using Services;
    using ViewModels;
    using global::OxyPlot.Services;

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
            var serviceLocator = ServiceLocator.Instance;
            serviceLocator.RegisterType<IOxyPlotService, OxyPlotService>();
        }
    }
}