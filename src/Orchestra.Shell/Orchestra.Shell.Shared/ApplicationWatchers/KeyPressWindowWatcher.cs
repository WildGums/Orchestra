// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyPressWindowWatcher.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public class KeyPressWindowWatcher
    {
        private static readonly bool[] _heldDown = new bool[256];

        private Action<KeyEventArgs> _keyDownHandler;
        private Action<KeyEventArgs> _keyUpHandler;
        private Action<KeyEventArgs> _previewKeyDownHandler;
        private Action<KeyEventArgs> _previewKeyUpHandler;

        public void WatchWindow(Window window)
        {
            window.PreviewKeyDown += OnPreviewKeyDown;
            window.KeyDown += OnKeyDown;
            window.PreviewKeyUp += OnPreviewKeyUp;
            window.KeyUp += OnKeyUp;
        }

        public void UnWatchWindow(Window window)
        {
            window.PreviewKeyDown -= OnPreviewKeyDown;
            window.KeyDown -= OnKeyDown;
            window.PreviewKeyUp -= OnPreviewKeyUp;
            window.KeyUp -= OnKeyUp;
        }

        public static bool IsKeyHeldDown(Key key)
        {
            var virtualKey = KeyInterop.VirtualKeyFromKey(key);
            return _heldDown[virtualKey];
        }

        public static bool IsCtrlHeldDown()
        {
            return IsKeyHeldDown(Key.LeftCtrl) || IsKeyHeldDown(Key.RightCtrl);
        }

        public static bool IsShiftHeldDown()
        {
            return IsKeyHeldDown(Key.LeftShift) || IsKeyHeldDown(Key.RightShift);
        }

        public static bool IsAltHeldDown()
        {
            return IsKeyHeldDown(Key.LeftAlt) || IsKeyHeldDown(Key.RightAlt);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            var virtualKey = KeyInterop.VirtualKeyFromKey(e.Key);
            _heldDown[virtualKey] = false;

            if (e.Handled)
            {
                return;
            }

            if (_keyUpHandler == null)
            {
                return;
            }

            var window = sender as Window;
            if (window == null)
            {
                return;
            }

            _keyUpHandler(e);
        }

        private void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }

            if (_previewKeyUpHandler == null)
            {
                return;
            }

            var window = sender as Window;
            if (window == null)
            {
                return;
            }

            _previewKeyUpHandler(e);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }

            if (_keyDownHandler == null)
            {
                return;
            }

            var window = sender as Window;
            if (window == null)
            {
                return;
            }

            _keyDownHandler(e);
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var virtualKey = KeyInterop.VirtualKeyFromKey(e.Key);
            _heldDown[virtualKey] = true;

            if (e.Handled)
            {
                return;
            }

            if (_previewKeyDownHandler == null)
            {
                return;
            }

            var window = sender as Window;
            if (window == null)
            {
                return;
            }

            _previewKeyDownHandler(e);
        }

        public void SetPreviewKeyDownHandler(Action<KeyEventArgs> handler)
        {
            _previewKeyDownHandler = handler;
        }

        public void SetKeyDownHandler(Action<KeyEventArgs> handler)
        {
            _keyDownHandler = handler;
        }

        public void SetPreviewKeyUpHandler(Action<KeyEventArgs> handler)
        {
            _previewKeyUpHandler = handler;
        }

        public void SetKeyUpHandler(Action<KeyEventArgs> handler)
        {
            _keyUpHandler = handler;
        }
    }
}