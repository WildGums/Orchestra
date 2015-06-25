// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBox.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using Catel.Windows;
    using ViewModels;

    public partial class MessageBox : DataWindow
    {
        #region Constructors
        public MessageBox()
            : this(null)
        {
        }

        public MessageBox(MessageBoxViewModel viewModel)
            : base(viewModel, DataWindowMode.Custom)
        {
            this.InitializeComponent();
            this.DisableCloseButton();
        }
        #endregion
    }
}