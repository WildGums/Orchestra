// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyPlotModule.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.OxyPlot
{
    using System;
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Messaging;
    using Nancy;
    using Orchestra.Services;
    using Services;
    using ViewModels;
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
        /// Initializes a new instance of the <see cref="T:Catel.Modules.ModuleBase`1" /> class.
        /// </summary>
        /// <param name="messageMediator">The message mediator.</param>
        public OxyPlotModule(IMessageMediator messageMediator)
            : base(Name)
        {
            messageMediator.Register(this, new Action<Tuple<ObservableCollection<int>, ObservableCollection<int>>>(OnPlot));
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

        /// <summary>
        /// Initializes the ribbon.
        /// <para />
        /// Use this method to hook up views to ribbon items.
        /// </summary>
        /// <param name="ribbonService">The ribbon service.</param>
        protected override void InitializeRibbon(IRibbonService ribbonService)
        {
            // Module specific
            // TODO: Register module specific ribbon items

            // View specific
            // TODO: Register view specific ribbon items
        }

        private void OnPlot(Tuple<ObservableCollection<int>, ObservableCollection<int>> data)
        {
            Log.Info("Received plot message");

            var plotViewModel = new PlotViewModel(data.Item1, data.Item2);

            var orchestraService = GetService<IOrchestraService>();
            orchestraService.ShowDocument(plotViewModel);
        }
    }
}