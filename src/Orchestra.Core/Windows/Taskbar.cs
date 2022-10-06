namespace Orchestra.Windows
{
    using System;
    using System.Drawing;
    using Orchestra.Win32;

    public enum TaskbarPosition
    {
        Unknown = -1,
        Left,
        Top,
        Right,
        Bottom,
    }

    /// <summary>
    /// Class Taskbar. This class cannot be inherited.
    /// </summary>
    /// <remarks>
    /// This code comes from http://winsharp93.wordpress.com/2009/06/29/find-out-size-and-position-of-the-taskbar/.
    /// </remarks>
    public sealed class Taskbar
    {
        private const string ClassName = "Shell_TrayWnd";

        public Taskbar()
        {
            var taskbarHandle = User32.FindWindow(Taskbar.ClassName, null);

            var data = APPBARDATA.NewAPPBARDATA();
            data.hWnd = taskbarHandle;
            var result = Shell32.SHAppBarMessage(ABM.GetTaskbarPos, ref data);
            if (result == IntPtr.Zero)
            {
                throw new InvalidOperationException();
            }

            Position = (TaskbarPosition) data.uEdge;
            Bounds = Rectangle.FromLTRB(data.rc.left, data.rc.top, data.rc.right, data.rc.bottom);

            result = Shell32.SHAppBarMessage(ABM.GetState, ref data);
            int state = result.ToInt32();
            AlwaysOnTop = (state & ABS.AlwaysOnTop) == ABS.AlwaysOnTop;
            AutoHide = (state & ABS.Autohide) == ABS.Autohide;
        }

        public Rectangle Bounds { get; private set; }
        public TaskbarPosition Position { get; private set; }

        public Point Location
        {
            get { return Bounds.Location; }
        }

        public Size Size
        {
            get { return Bounds.Size; }
        }

        //Always returns false under Windows 7
        public bool AlwaysOnTop { get; private set; }
        public bool AutoHide { get; private set; }
    }
}
