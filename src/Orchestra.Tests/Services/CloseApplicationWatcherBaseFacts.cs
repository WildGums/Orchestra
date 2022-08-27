namespace Orchestra.Tests
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    internal class CloseApplicationWatcherBaseFacts
    {
        [TestCase]
        public async Task VerifyClosingClosedOperationsAreExecutingAsync()
        {
            bool isWatcherCompleted = false;
            using (var cts = new CancellationTokenSource(10000))
            {
                // tested object
                var watcher = new TestCloseApplicationWatcher();

                // Use a semaphore to prevent the [TestMethod] from returning prematurely.
                using (var semaphore = await RunStaThreadAsync(() =>
                {
                    var window = new System.Windows.Window();
                    
                    // access handler method
                    var onWindowClosing = typeof(CloseApplicationWatcherBase).GetMethod("OnWindowClosing", BindingFlags.Static | BindingFlags.NonPublic);

                    Assert.IsNotNull(onWindowClosing);

                    var cancelEventArgs = new CancelEventArgs();
                    var cancelEventArgsRetry = new CancelEventArgs();

                    window.Closing += (sender, e) =>
                    {
                        onWindowClosing.Invoke(watcher, new object[] { window, cancelEventArgsRetry });
                    };

                    onWindowClosing.Invoke(watcher, new object[] { window, cancelEventArgs });

                    while (!cts.IsCancellationRequested)
                    {
                        isWatcherCompleted = watcher.IsClosedRun && watcher.IsClosingRun;
                        if (isWatcherCompleted)
                        {
                            break;
                        }
                    }
                }))
                {
                    await semaphore.WaitAsync();

                    Assert.IsTrue(watcher.IsClosedRun);
                    Assert.IsTrue(watcher.IsClosingRun);
                }
            }
        }

        private async Task<SemaphoreSlim> RunStaThreadAsync(Action action)
        {
            var semaphore = new SemaphoreSlim(1);
            await semaphore.WaitAsync();

            var thread = new Thread(() =>
            {
                try
                {
                    // Verify new thread able to host UI component
                    Assert.IsTrue(Thread.CurrentThread.GetApartmentState() == ApartmentState.STA);

                    action();

                    semaphore.Release();
                }
                catch (InvalidOperationException)
                {
                    // Handle dispatcher access exception happens in test
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            return semaphore;
        }
    }
}
