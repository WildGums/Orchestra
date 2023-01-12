namespace Orchestra.Behaviors
{
    using System;
    using System.Windows;
    using Catel.Logging;
    using Catel.Windows.Interactivity;
    using Orc.Controls;

    public class RememberWindowSize : BehaviorBase<Window>
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public bool MakeWindowResizable
        {
            get { return (bool)GetValue(MakeWindowResizableProperty); }
            set { SetValue(MakeWindowResizableProperty, value); }
        }

        public static readonly DependencyProperty MakeWindowResizableProperty = DependencyProperty.Register(nameof(MakeWindowResizable), 
            typeof(bool), typeof(RememberWindowSize), new PropertyMetadata(true));


        public bool RememberWindowState
        {
            get { return (bool)GetValue(RememberWindowStateProperty); }
            set { SetValue(RememberWindowStateProperty, value); }
        }

        public static readonly DependencyProperty RememberWindowStateProperty = DependencyProperty.Register(nameof(RememberWindowState), 
            typeof(bool), typeof(RememberWindowSize), new PropertyMetadata(true));

        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();

            var window = AssociatedObject;
            var windowType = window.GetType().Name;

            if (MakeWindowResizable && window.ResizeMode == ResizeMode.NoResize)
            {
                Log.Debug($"Setting window ResizeMode to CanResize and SizeToContent to Manual of '{windowType}'");

                window.SetCurrentValue(Window.SizeToContentProperty, SizeToContent.Manual);
                window.SetCurrentValue(Window.ResizeModeProperty, ResizeMode.CanResize);
            }

            window.LoadWindowSize(RememberWindowState);

            switch (window.WindowStartupLocation)
            {
                case WindowStartupLocation.Manual:
                    break;

                case WindowStartupLocation.CenterScreen:
                    window.CenterWindowToScreen();
                    break;

                case WindowStartupLocation.CenterOwner:
                    window.CenterWindowToParent();
                    break;

                default:
                    throw Log.ErrorAndCreateException(_ => new ArgumentOutOfRangeException(nameof(window.WindowStartupLocation)), string.Empty);
            }

            window.Closed += OnWindowClosed;
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            var window = AssociatedObject;

            window.Closed -= OnWindowClosed;

            base.OnAssociatedObjectUnloaded();
        }

        private void OnWindowClosed(object? sender, EventArgs e)
        {
            AssociatedObject.SaveWindowSize();
        }
    }
}
