// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WpfScreen.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Interop;
    using Point = System.Windows.Point;

    public class WpfScreen
    {
        private readonly MonitorInfo _screen;
        #region Fields

        #endregion

        #region Constructors

        internal WpfScreen(MonitorInfo monitorInfo)
        {
            _screen = monitorInfo;
        }
        #endregion

        #region Properties
        public static WpfScreen Primary
        {
            get { return new WpfScreen(MonitorInfo.GetPrimaryMonitor()); }
        }

        public Rect DeviceBounds
        {
            get { return new Rect(_screen.MonitorArea.X, _screen.MonitorArea.Y, _screen.MonitorArea.Width, _screen.MonitorArea.Height); }
        }

        public Rect WorkingArea
        {
            get { return new Rect(_screen.WorkingArea.X, _screen.WorkingArea.Y, _screen.WorkingArea.Width, _screen.WorkingArea.Height); }
        }

        public bool IsPrimary
        {
            get { return _screen.IsPrimary; }
        }

        public string DeviceName
        {
            get { return _screen.DeviceName; }
        }

        public string FriendlyName
        {
            get { return _screen.FriendlyName; }
        }

        #endregion

        #region Methods
        public static IEnumerable<WpfScreen> AllScreens()
        {
            foreach (var monitorInfo in MonitorInfo.GetAllMonitors())
            {
                yield return new WpfScreen(monitorInfo);
            }
        }

        public static WpfScreen GetScreenFrom(Window window)
        {
            return new WpfScreen(MonitorInfo.GetMonitorFromWindow(window));
        }

        public static WpfScreen GetScreenFrom(Point point)
        {
            //int x = (int)Math.Round(point.X);
            //int y = (int)Math.Round(point.Y);

            //// are x,y device-independent-pixels ??
            //var drawingPoint = new System.Drawing.Point(x, y);
            //Screen screen = Screen.FromPoint(drawingPoint);
            //var wpfScreen = new WpfScreen(screen);

            //return wpfScreen;
            throw new NotImplementedException();
        }

        #endregion
    }
}
