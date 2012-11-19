// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyPlotModule.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.OxyPlot
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using Catel.IoC;
    using Catel.Logging;
    using Nancy;
    using Services;
    using global::OxyPlot.Services;
    using Nancy.Hosting.Wcf;

    /// <summary>
    /// The oxyplot module.
    /// </summary>
    public class OxyPlotModule : ModuleBase
    {
        /// <summary>
        /// The module name.
        /// </summary>
        public const string Name = "OxyPlot";

        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The base uri.
        /// </summary>
        private static readonly Uri BaseUri = new Uri("http://localhost:4242/oxyplot/");

        /// <summary>
        /// The web service host.
        /// </summary>
        private WebServiceHost _host;

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
            var serviceLocator = ServiceLocator.Default;
            serviceLocator.RegisterType<IOxyPlotService, OxyPlotService>();

            Log.Info("Starting OxyPlot web service at address '{0}'", BaseUri);

            _host = new WebServiceHost(new NancyWcfGenericService(new DefaultNancyBootstrapper()), BaseUri);
            _host.AddServiceEndpoint(typeof(NancyWcfGenericService), new WebHttpBinding(), string.Empty);
            _host.Open();

            Log.Info("Started OxyPlot web service at address '{0}'", BaseUri);
        }
    }
}