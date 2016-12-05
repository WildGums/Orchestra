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
        }

        private async Task ShowCustomDialogExecuteAsync()
        {
            var result = await _uiVisualizerService.ShowDialogAsync<ExampleDialogViewModel>();
            var messageResult = await _messageService.ShowInformationAsync($"The result of the custom dialog is {result}");
        }

        public TaskCommand OnShowCustomDialogExecute { get; private set; }
    }
}
