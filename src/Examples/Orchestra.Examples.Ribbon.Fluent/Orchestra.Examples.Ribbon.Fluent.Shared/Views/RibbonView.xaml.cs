// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonView.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon.Views
{
    public partial class RibbonView 
    {
        #region Constructors
        public RibbonView()
        {
            InitializeComponent();

            ribbon.AddAboutButton();
        }

        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            backstageTabControl.SetCurrentValue(DataContextProperty, ViewModel);
        }
        #endregion
    }
}