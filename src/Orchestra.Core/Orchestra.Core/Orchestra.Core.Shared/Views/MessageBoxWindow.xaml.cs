// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBox.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using System;
    using Catel.Services;
    using Catel.Windows;
    using ViewModels;

    public partial class MessageBoxWindow
    {
        #region Constructors
        public MessageBoxWindow()
            : this(null)
        {
        }

        public MessageBoxWindow(MessageBoxViewModel viewModel)
            : base(viewModel, DataWindowMode.Custom)
        {
            InitializeComponent();

            if (viewModel.Button == MessageButton.YesNo)
            {
                this.DisableCloseButton();
            }
        }
        #endregion
    }
}