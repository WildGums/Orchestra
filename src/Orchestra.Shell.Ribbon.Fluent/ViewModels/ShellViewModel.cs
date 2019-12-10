// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System.Reflection;
    using Catel;
    using Catel.MVVM;
    using Catel.Reflection;
    using Orchestra.Services;

    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel(IShellConfigurationService shellConfigurationService)
        {
            Argument.IsNotNull(() => shellConfigurationService);

            DeferValidationUntilFirstSaveCall = shellConfigurationService.DeferValidationUntilFirstSaveCall;

            var assembly = Assembly.GetEntryAssembly();
            Title = assembly.Title();
        }
    }
}
