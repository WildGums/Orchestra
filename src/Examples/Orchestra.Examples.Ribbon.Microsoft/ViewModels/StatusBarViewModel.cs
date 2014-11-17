// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusBarViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon.ViewModels
{
    using Catel.MVVM;

    public class StatusBarViewModel : ViewModelBase
    {
        #region Properties
        public override string Title
        {
            get { return "Status bar title binding"; }
        }
        #endregion

        public bool EnableAutomaticUpdates { get; set; }
    }
}