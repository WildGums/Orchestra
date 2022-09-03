namespace Orchestra.Tests
{
    using System.Threading.Tasks;

    internal class TestCloseApplicationWatcher : CloseApplicationWatcherBase
    {
        private readonly bool _cancel;

        public TestCloseApplicationWatcher(bool cancel)
        {
            _cancel = cancel;

            // Required for unit testing
            Reset();
        }

        public bool IsClosedRun { get; set; }

        public bool IsClosingRun { get; set; }

        protected override async Task ClosedAsync()
        {
            IsClosedRun = true;
        }

        protected override async Task<bool> ClosingAsync()
        {
            IsClosingRun = true;
            return !_cancel;
        }
    }
}
