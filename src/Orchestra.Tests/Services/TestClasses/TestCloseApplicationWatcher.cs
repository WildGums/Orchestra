namespace Orchestra.Tests
{
    using System.Threading.Tasks;

    internal class TestCloseApplicationWatcher : CloseApplicationWatcherBase
    {
        public TestCloseApplicationWatcher()
        {

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
            return true;
        }
    }
}
