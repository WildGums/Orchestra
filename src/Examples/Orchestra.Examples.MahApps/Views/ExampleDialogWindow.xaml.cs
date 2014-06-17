// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleDialogWindow.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.MahApps.Views
{
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for ExampleDialogWindow.xaml.
    /// </summary>
    public partial class ExampleDialogWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleDialogWindow"/> class.
        /// </summary>
        public ExampleDialogWindow()
            : base(DataWindowMode.Custom)
        {
            AddCustomButton(new DataWindowButton("Save anyway", ExecuteOk, OnOkCanExecute));
            AddCustomButton(new DataWindowButton("Cancel", ExecuteCancel, OnCancelCanExecute));

            InitializeComponent();
        }
        #endregion
    }
}