// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Taskbar.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Windows
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

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
        #region Constants
        private const string ClassName = "Shell_TrayWnd";
        #endregion

        #region Constructors
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
        #endregion

        #region Properties
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
        #endregion
    }

    public enum ABM : uint
    {
        New = 0x00000000,
        Remove = 0x00000001,
        QueryPos = 0x00000002,
        SetPos = 0x00000003,
        GetState = 0x00000004,
        GetTaskbarPos = 0x00000005,
        Activate = 0x00000006,
        GetAutoHideBar = 0x00000007,
        SetAutoHideBar = 0x00000008,
        WindowPosChanged = 0x00000009,
        SetState = 0x0000000A,
    }

    public enum ABE : uint
    {
        Left = 0,
        Top = 1,
        Right = 2,
        Bottom = 3
    }

    public static class ABS
    {
        #region Constants
        public const int Autohide = 0x0000001;
        public const int AlwaysOnTop = 0x0000002;
        #endregion
    }

    public static class Shell32
    {
        #region Methods
        [DllImport("shell32.dll", SetLastError = true)]
        public static extern IntPtr SHAppBarMessage(ABM dwMessage, [In] ref APPBARDATA pData);
        #endregion
    }

    public static class User32
    {
        #region Methods
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct APPBARDATA
    {
        public static APPBARDATA NewAPPBARDATA()
        {
            var abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(typeof(APPBARDATA));
            return abd;
        }

        #region Fields
        public int cbSize;
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public ABE uEdge;
        public RECT rc;
        public int lParam;
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        #region Fields
        public int left;
        public int top;
        public int right;
        public int bottom;
        #endregion
    }
}