// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Examples.OxyPlotConsumer
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using System.Net;
    using OxyPlot.Models;

    internal class Program
    {
        private const string Url = "http://localhost:4242/oxyplot/plot";

        #region Methods
        private static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Press any key to make the request");
                Console.ReadKey();

                Console.WriteLine("Making the request to " + Url);

                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(Url);
                    request.ContentType = "application/json";
                    request.Method = "POST";

                    using (var requestStream = request.GetRequestStream())
                    {
                        var oxyPlotModel = CreateLinePlotModel();
                        var json = JsonConvert.SerializeObject(oxyPlotModel);

                        using (var streamWriter = new StreamWriter(requestStream))
                        {
                            streamWriter.Write(json);
                        }
                    }

                    request.GetResponse();

                    Console.WriteLine("Successfully made the request");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to make the request: " + ex.Message);
                }
            }
        }

        private static OxyPlotModel CreateLinePlotModel()
        {
            var model = new OxyPlotModel(SerieTypes.Line, "Test", "legend");

            // Create two line series (markers are hidden by default)
            var series1 = new LineSeries("Series 1"); // { MarkerType = MarkerType.Circle };
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(10, 18));
            series1.Points.Add(new DataPoint(20, 12));
            series1.Points.Add(new DataPoint(30, 8));
            series1.Points.Add(new DataPoint(40, 15));

            var series2 = new LineSeries("Series 2"); // { MarkerType = MarkerType.Square };
            series2.Points.Add(new DataPoint(0, 4));
            series2.Points.Add(new DataPoint(10, 12));
            series2.Points.Add(new DataPoint(20, 16));
            series2.Points.Add(new DataPoint(30, 25));
            series2.Points.Add(new DataPoint(40, 5));

            // Add the series to the plot model
            model.Series.Add(series1);
            model.Series.Add(series2);

            return model;
        }
        #endregion
    }
}