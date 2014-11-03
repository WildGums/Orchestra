// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonView.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon.Views
{
    /// <summary>
    /// Interaction logic for RibbonView.xaml.
    /// </summary>
    public partial class RibbonView 
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonView"/> class.
        /// </summary>
        public RibbonView()
        {
            InitializeComponent();

            ribbon.AddAboutButton();
        }

        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            backstageTabControl.DataContext = ViewModel;
        }
        #endregion
    }
}