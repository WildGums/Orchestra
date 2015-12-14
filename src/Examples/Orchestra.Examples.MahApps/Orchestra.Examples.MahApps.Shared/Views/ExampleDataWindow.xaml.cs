// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleDataWindow.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Examples.MahApps.Views
{
    using Catel.Windows;

    public sealed partial class ExampleDataWindow
    {
        #region Constructors
        public ExampleDataWindow()
            //: base(DataWindowMode.Custom)
        {
            //AddCustomButton(new DataWindowButton("Save anyway", async () => await ExecuteOkAsync(), OnOkCanExecute));
            //AddCustomButton(new DataWindowButton("Cancel", async () => await ExecuteCancelAsync(), OnCancelCanExecute));

            InitializeComponent();
        }
        #endregion
    }
}