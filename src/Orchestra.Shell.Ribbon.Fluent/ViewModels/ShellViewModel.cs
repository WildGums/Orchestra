namespace Orchestra.ViewModels
{
    using System;
    using Catel.MVVM;
    using Catel.Reflection;

    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel(IShellConfigurationService shellConfigurationService)
        {
            ArgumentNullException.ThrowIfNull(shellConfigurationService);

            ValidateUsingDataAnnotations = shellConfigurationService.ValidateUsingDataAnnotations;

            var assembly = AssemblyHelper.GetRequiredEntryAssembly();
            Title = assembly.Title() ?? string.Empty;
        }
    }
}
