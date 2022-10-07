namespace Orchestra.ViewModels
{
    using System;
    using System.Reflection;
    using Catel.MVVM;
    using Catel.Reflection;
    using Orchestra.Services;

    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel(IShellConfigurationService shellConfigurationService)
        {
            ArgumentNullException.ThrowIfNull(shellConfigurationService);

            DeferValidationUntilFirstSaveCall = shellConfigurationService.DeferValidationUntilFirstSaveCall;

            var assembly = Assembly.GetEntryAssembly();
            Title = assembly.Title() ?? string.Empty;
        }
    }
}
