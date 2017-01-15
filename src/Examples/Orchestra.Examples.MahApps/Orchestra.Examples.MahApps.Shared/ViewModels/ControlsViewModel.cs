namespace Orchestra.Examples.MahApps.ViewModels
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;

    public class ControlsViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IMessageService _messageService;

        public ControlsViewModel(IUIVisualizerService uiVisualizerService, IMessageService messageService)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => messageService);

            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;

            OnShowCustomDialogExecute = new TaskCommand(ShowCustomDialogExecuteAsync);
            OnShowCloseDialogExecute = new TaskCommand(ShowCloseDialogExecuteAsync);
            OnShowOkCancelDialogExecute = new TaskCommand(ShowOkCancelDialogExecuteAsync);
            OnShowOkCancelApplyDialogExecute = new TaskCommand(ShowOkCancelApplyDialogExecuteAsync);
        }

        public TaskCommand OnShowCustomDialogExecute { get; private set; }

        public TaskCommand OnShowCloseDialogExecute { get; private set; }

        public TaskCommand OnShowOkCancelDialogExecute { get; private set; }

        public TaskCommand OnShowOkCancelApplyDialogExecute { get; private set; }

        private async Task ShowOkCancelApplyDialogExecuteAsync()
        {
            var result = await _uiVisualizerService.ShowDialogAsync<ExampleDialogOkCancelApplyViewModel>();
            await ShowResultInMessageBoxAsync(result);
        }

        private async Task ShowOkCancelDialogExecuteAsync()
        {
            var result = await _uiVisualizerService.ShowDialogAsync<ExampleDialogOkCancelViewModel>();
            await ShowResultInMessageBoxAsync(result);
        }

        private async Task ShowCloseDialogExecuteAsync()
        {
            var result = await _uiVisualizerService.ShowDialogAsync<ExampleDialogCloseViewModel>();
            await ShowResultInMessageBoxAsync(result);
        }

        private async Task ShowResultInMessageBoxAsync(bool? result)
        {
            var part = !result.HasValue ? "null" : result.Value ? "true" : "false";
            await _messageService.ShowInformationAsync($"The result of the custom dialog is '{part}'");
        }

        private async Task ShowCustomDialogExecuteAsync()
        {
            var result = await _uiVisualizerService.ShowDialogAsync<ExampleDialogViewModel>();
            var messageResult = await _messageService.ShowInformationAsync($"The result of the custom dialog is {result}");
        }
    }
}
