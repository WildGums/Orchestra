namespace Orchestra.Examples.Ribbon.Services
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Services;
    using Orc.Notifications;

    internal class UserMessageCloseApplicationWatcher : CloseApplicationWatcherBase
    {
        private readonly IMessageService _messageService;
        private readonly INotificationService _notificationService;

        public UserMessageCloseApplicationWatcher(IMessageService messageService, INotificationService notificationService)
        {
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => notificationService);

            _messageService = messageService;
            _notificationService = notificationService;
        }

        protected override async Task<bool> ClosingAsync()
        {
            var result = await _messageService.ShowAsync("Are you sure you want to close example?", "Closing", MessageButton.YesNo, MessageImage.Question);
            return result == MessageResult.Yes;
        }

        protected override async Task ClosedAsync()
        {
            _notificationService.ShowNotification(new Notification
            {
                Title = "Closing approved",
                Message = "User approved closing the app, closing within 5 seconds",
            });

            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
}
