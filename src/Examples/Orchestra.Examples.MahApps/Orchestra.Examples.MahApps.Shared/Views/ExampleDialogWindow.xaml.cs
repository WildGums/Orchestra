// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleDialogWindow.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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
            AddCustomButton(DataWindowButton.FromAsync("Save anyway", ExecuteOkAsync, OnOkCanExecute));
            AddCustomButton(DataWindowButton.FromAsync("Cancel", ExecuteCancelAsync, OnCancelCanExecute));

            InitializeComponent();
        }
        #endregion
    }
}