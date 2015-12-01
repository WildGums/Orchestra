// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyPressApplicationWatcherBase.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    public class KeyPressApplicationWatcherBase : ApplicationWatcherBase
    {
        private static readonly IList<KeyPressApplicationWatcherBase> Watchers = new List<KeyPressApplicationWatcherBase>();

        public KeyPressApplicationWatcherBase()
        {
            Watchers.Add(this);

            EnqueueShellActivatedAction(Subscribe);
        }

        private static void Subscribe(Window window)
        {
            var windowWatcher = new KeyPressWindowWatcher();
            windowWatcher.WatchWindow(window);

            windowWatcher.SetPreviewKeyDownHandler(e =>
            {
                foreach (var watcher in Watchers)
                {
                    watcher.OnPreviewKeyDown(e);
                }
            });

            windowWatcher.SetKeyDownHandler(e =>
            {
                foreach (var watcher in Watchers)
                {
                    watcher.OnKeyDown(e);
                }
            });

            windowWatcher.SetPreviewKeyUpHandler(e =>
            {
                foreach (var watcher in Watchers)
                {
                    watcher.OnPreviewKeyUp(e);
                }
            });

            windowWatcher.SetKeyUpHandler(e =>
            {
                foreach (var watcher in Watchers)
                {
                    watcher.OnKeyUp(e);
                }
            });
        }

        protected virtual void OnPreviewKeyDown(KeyEventArgs e)
        {
        }

        protected virtual void OnKeyDown(KeyEventArgs e)
        {
        }

        protected virtual void OnPreviewKeyUp(KeyEventArgs e)
        {
        }

        protected virtual void OnKeyUp(KeyEventArgs e)
        {
        }
    }
}