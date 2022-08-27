namespace Orchestra.Tests
{
    using System.Threading.Tasks;

    internal class TestCloseApplicationWatcher : CloseApplicationWatcherBase
    {
        /// <summary>
        /// For testing we call watcher in separate thread. As it's used async await inside OnClosingWindowAsync we should
        /// prevent thread from exit prematurely.
        /// </summary>
        private readonly TaskCompletionSource _taskCompletionSource;

        public TestCloseApplicationWatcher(TaskCompletionSource taskCompletionSource)
        {
            _taskCompletionSource = taskCompletionSource;
        }

        public bool IsClosedRun { get; set; }

        public bool IsClosingRun { get; set; }

        protected override async Task ClosedAsync()
        {
            _taskCompletionSource.SetResult();
            IsClosedRun = true;
        }

        protected override async Task<bool> ClosingAsync()
        {
            IsClosingRun = true;
            return true;
        }
    }
}
