// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBox.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
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