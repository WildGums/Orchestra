// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonView.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon.Views
{
    using Catel.Windows.Controls;

    /// <summary>
    /// Interaction logic for RibbonView.xaml.
    /// </summary>
    public partial class RibbonView : UserControl
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonView"/> class.
        /// </summary>
        public RibbonView()
        {
            InitializeComponent();

            ribbon.AddMinimizeAndMaximizeButtons();
            ribbon.AddAboutButton();
        }
        #endregion
    }
}