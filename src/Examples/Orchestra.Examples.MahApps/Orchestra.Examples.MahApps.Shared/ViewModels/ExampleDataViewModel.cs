// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleDataViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.MahApps.ViewModels
{
    using Catel.MVVM;

    public class ExampleDataViewModel : ViewModelBase
    {
        #region Properties
        public override string Title
        {
            get { return "Bindable title"; }
        }
        #endregion
    }
}