// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WpfScreen.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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
        #region Fields
        private readonly Screen _screen;
        #endregion

        #region Constructors
        internal WpfScreen(Screen screen)
        {
            _screen = screen;
        }
        #endregion

        #region Properties
        public static WpfScreen Primary
        {
            get { return new WpfScreen(Screen.PrimaryScreen); }
        }

        public Rect DeviceBounds
        {
            get { return GetRect(_screen.Bounds); }
        }

        public Rect WorkingArea
        {
            get { return GetRect(_screen.WorkingArea); }
        }

        public bool IsPrimary
        {
            get { return _screen.Primary; }
        }

        public string DeviceName
        {
            get { return _screen.DeviceName; }
        }
        #endregion

        #region Methods
        public static IEnumerable<WpfScreen> AllScreens()
        {
            foreach (Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                yield return new WpfScreen(screen);
            }
        }

        public static WpfScreen GetScreenFrom(Window window)
        {
            var windowInteropHelper = new WindowInteropHelper(window);
            var screen = Screen.FromHandle(windowInteropHelper.Handle);
            var wpfScreen = new WpfScreen(screen);
            return wpfScreen;
        }

        public static WpfScreen GetScreenFrom(Point point)
        {
            int x = (int) Math.Round(point.X);
            int y = (int) Math.Round(point.Y);

            // are x,y device-independent-pixels ??
            var drawingPoint = new System.Drawing.Point(x, y);
            Screen screen = Screen.FromPoint(drawingPoint);
            var wpfScreen = new WpfScreen(screen);

            return wpfScreen;
        }

        private Rect GetRect(Rectangle value)
        {
            // should x, y, width, height be device-independent-pixels ??
            return new Rect
            {
                X = value.X,
                Y = value.Y,
                Width = value.Width,
                Height = value.Height
            };
        }
        #endregion
    }
}