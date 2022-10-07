namespace Orchestra.ViewModels
{
    using System;
    using Catel.MVVM;
    using Catel.Reflection;
    using Orchestra.Services;

    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel(IShellConfigurationService shellConfigurationService)
        {
            ArgumentNullException.ThrowIfNull(shellConfigurationService);

            DeferValidationUntilFirstSaveCall = shellConfigurationService.DeferValidationUntilFirstSaveCall;

            var assembly = AssemblyHelper.GetRequiredEntryAssembly();
            Title = assembly.Title() ?? string.Empty;
        }
    }
}
