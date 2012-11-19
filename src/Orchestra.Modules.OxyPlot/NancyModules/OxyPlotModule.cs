// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyPlotModule.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.OxyPlot.NancyModules
{
    using System;
    using System.IO;
    using Catel.IoC;
    using Catel.Logging;
    using Nancy;
    using Newtonsoft.Json;
    using global::OxyPlot.Models;
    using global::OxyPlot.Services;

    /// <summary>
    /// Nancy module handling all web requests.
    /// </summary>
    public class OxyPlotModule : NancyModule
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IOxyPlotService _service;

        /// <summary>
        /// Initializes the <see cref="OxyPlotModule"/> class.
        /// </summary>
        public OxyPlotModule()
        {
            var serviceLocator = ServiceLocator.Default;
            _service = serviceLocator.ResolveType<IOxyPlotService>();

            Get["/"] = parameters =>
            {
                return 200;
            };

            Post["/plot"] = parameters =>
            {
                try
                {
                    string json = null;
                    using (var reader = new StreamReader(Request.Body))
                    {
                        json = reader.ReadToEnd();
                    }

                    var plotModel = JsonConvert.DeserializeObject<OxyPlotModel>(json);
                    _service.Plot(plotModel);

                    return 200;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to handle incomding request /plot");

                    return 503;
                }
            };
        }
    }
}