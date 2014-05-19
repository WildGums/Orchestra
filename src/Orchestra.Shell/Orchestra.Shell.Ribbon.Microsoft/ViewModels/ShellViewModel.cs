// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System.Reflection;
    using Catel.MVVM;
    using Catel.Reflection;

    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel()
        {
            var assembly = Assembly.GetEntryAssembly();
            Title = assembly.Title();
        }
    }
}