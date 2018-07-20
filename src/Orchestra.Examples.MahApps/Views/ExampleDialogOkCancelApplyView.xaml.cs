// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleDialogOkCancelApplyView.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.MahApps.Views
{
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for ExampleDialogOkCancelApplyView..xaml.
    /// </summary>
    public partial class ExampleDialogOkCancelApplyView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleDialogOkCancelApplyView"/> class.
        /// </summary>
        public ExampleDialogOkCancelApplyView()
            : base(DataWindowMode.OkCancelApply)
        {
            InitializeComponent();
        }
        #endregion
    }
}