// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleDialogViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.MahApps.ViewModels
{
    using Catel.MVVM;

    public class ExampleDialogViewModel : ViewModelBase
    {
        public override string Title
        {
            get { return "Bindable title"; }
        }
    }
}