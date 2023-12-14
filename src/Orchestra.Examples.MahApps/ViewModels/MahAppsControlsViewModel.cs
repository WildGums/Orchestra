namespace Orchestra.Examples.MahApps.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Services;

    public class MahAppsControlsViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IMessageService _messageService;

        public MahAppsControlsViewModel(IUIVisualizerService uiVisualizerService, IMessageService messageService)
        {
            ArgumentNullException.ThrowIfNull(uiVisualizerService);
            ArgumentNullException.ThrowIfNull(messageService);

            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;

            ShowCustomDialog = new TaskCommand(OnShowCustomDialogExecuteAsync);
            ShowCloseDialog = new TaskCommand(OnShowCloseDialogExecuteAsync);
            ShowOkCancelDialog = new TaskCommand(OnShowOkCancelDialogExecuteAsync);
            ShowOkCancelApplyDialog = new TaskCommand(OnShowOkCancelApplyDialogExecuteAsync);
        }

        public TaskCommand ShowCustomDialog { get; private set; }

        public TaskCommand ShowCloseDialog { get; private set; }

        public TaskCommand ShowOkCancelDialog { get; private set; }

        public TaskCommand ShowOkCancelApplyDialog { get; private set; }

        private async Task OnShowOkCancelApplyDialogExecuteAsync()
        {
            var result = await _uiVisualizerService.ShowDialogAsync<ExampleDialogOkCancelApplyViewModel>();
            await ShowResultInMessageBoxAsync(result);
        }

        private async Task OnShowOkCancelDialogExecuteAsync()
        {
            var result = await _uiVisualizerService.ShowDialogAsync<ExampleDialogOkCancelViewModel>();
            await ShowResultInMessageBoxAsync(result);
        }

        private async Task OnShowCloseDialogExecuteAsync()
        {
            var result = await _uiVisualizerService.ShowDialogAsync<ExampleDialogCloseViewModel>();
            await ShowResultInMessageBoxAsync(result);
        }

        private async Task ShowResultInMessageBoxAsync(UIVisualizerResult result)
        {
            var part = !result.DialogResult.HasValue ? "null" : result.DialogResult.Value ? "true" : "false";
            await _messageService.ShowInformationAsync($"The result of the custom dialog is '{part}'");
        }

        private async Task OnShowCustomDialogExecuteAsync()
        {
            var result = await _uiVisualizerService.ShowDialogAsync<ExampleDialogViewModel>();
            var messageResult = await _messageService.ShowInformationAsync($"The result of the custom dialog is {result}");
        }
    }
}
