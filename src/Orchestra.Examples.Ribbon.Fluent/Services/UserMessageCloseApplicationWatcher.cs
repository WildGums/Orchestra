namespace Orchestra.Examples.Ribbon.Services
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Services;

    internal class UserMessageCloseApplicationWatcher : CloseApplicationWatcherBase
    {
        private readonly IMessageService _messageService;

        public UserMessageCloseApplicationWatcher(IMessageService messageService)
        {
            Argument.IsNotNull(() => messageService);

            _messageService = messageService;
        }

        protected override async Task<bool> ClosingAsync()
        {
            var result = await _messageService.ShowAsync("Are you sure you want to close example?", "Closing", MessageButton.OKCancel, MessageImage.Question);
            return result == MessageResult.Yes;
        }
    }
}
