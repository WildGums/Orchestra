// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaElementThreadFactory.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------



#if NET

namespace Orchestra.Controls
{
    using System;
    using System.Threading;
    using System.Windows.Media;
    using System.Windows.Threading;
    using Catel;

    /// <summary>
    /// Factory that allows the creation of media elements on a worker thread.
    /// </summary>
    public static class MediaElementThreadFactory
    {
        #region Fields
        private static readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        #endregion

        #region Methods
        /// <summary>
        /// Creates the media element on worker thread.
        /// <para />
        /// Note that the <see cref="MediaElementThreadInfo"/> implements <see cref="IDisposable"/>.
        /// </summary>
        /// <returns>The media element thread info.</returns>
        public static MediaElementThreadInfo CreateMediaElementsOnWorkerThread(Func<Visual> createVisual)
        {
            Argument.IsNotNull("createVisual", createVisual);

            var visual = new HostVisual();

            var thread = new Thread(WorkerThread);

            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start(new object[] { visual, createVisual });

            _autoResetEvent.WaitOne();

            return new MediaElementThreadInfo(visual, thread);
        }

        private static void WorkerThread(object arg)
        {
            try
            {
                var objects = (object[])arg;

                var hostVisual = (HostVisual)objects[0];
                var createVisual = (Func<Visual>)objects[1];

                var indeterminateSource = new VisualTargetPresentationSource(hostVisual);

                _autoResetEvent.Set();

                indeterminateSource.RootVisual = createVisual();

                Dispatcher.Run();
            }
            catch
            {
            }
        }
        #endregion
    }
}

#endif