// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleDialogCloseView.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.MahApps.Views
{
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for ExampleDialogCloseView.xaml.
    /// </summary>
    public partial class ExampleDialogCloseView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleDialogCloseView"/> class.
        /// </summary>
        public ExampleDialogCloseView()
            : base(DataWindowMode.Close)
        {
            InitializeComponent();
        }
        #endregion
    }
}