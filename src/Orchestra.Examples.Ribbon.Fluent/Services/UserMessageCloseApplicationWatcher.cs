namespace Orchestra.Examples.Ribbon.Services;

using System;
using System.Threading.Tasks;
using Catel.Services;
using Orc.Notifications;

internal class UserMessageCloseApplicationWatcher : CloseApplicationWatcherBase
{
    private readonly IMessageService _messageService;
    private readonly INotificationService _notificationService;
    private readonly ILanguageService _languageService;

    public UserMessageCloseApplicationWatcher(IMessageService messageService, INotificationService notificationService,
        ILanguageService languageService, IDispatcherService dispatcherService, IMainWindowService mainWindowService)
        : base(messageService, dispatcherService, mainWindowService)
    {
        _messageService = messageService;
        _notificationService = notificationService;
        _languageService = languageService;
    }

    protected override async Task<bool> ClosingAsync()
    {
        var result = await _messageService.ShowAsync(
            _languageService.GetRequiredString("Orchestra_Examples_Ribbon_UserMessage_AreYouSure"),
            _languageService.GetRequiredString("Orchestra_Examples_Ribbon_UserMessage_ClosingTitle"),
            MessageButton.YesNo, MessageImage.Question);
        return result == MessageResult.Yes;
    }

    protected override async Task ClosedAsync()
    {
        _notificationService.ShowNotification(new Notification
        {
            Title = _languageService.GetRequiredString("Orchestra_Examples_Ribbon_UserMessage_ClosingApproved"),
            Message = _languageService.GetRequiredString("Orchestra_Examples_Ribbon_UserMessage_ClosingApprovedMessage"),
        });

        await Task.Delay(TimeSpan.FromSeconds(5));
    }
}
