namespace Orchestra.ViewModels
{
    using System;
    using Catel;
    using Catel.MVVM;
    using Orchestra.Services;

    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel(IShellConfigurationService shellConfigurationService)
        {
            ArgumentNullException.ThrowIfNull(shellConfigurationService);

            DeferValidationUntilFirstSaveCall = shellConfigurationService.DeferValidationUntilFirstSaveCall;
        }
    }
}
