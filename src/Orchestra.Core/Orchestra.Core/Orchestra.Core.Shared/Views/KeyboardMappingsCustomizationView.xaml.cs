// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyboardMappingsCustomizationView.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using System.Windows;
    using System.Windows.Input;
    using Catel.Windows;
    using Catel.Windows.Controls;
    using ViewModels;
    using InputGesture = Catel.Windows.Input.InputGesture;

    /// <summary>
    /// Interaction logic for KeyboardMappingsView.xaml.
    /// </summary>
    public partial class KeyboardMappingsCustomizationView : UserControl
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardMappingsCustomizationView"/> class.
        /// </summary>
        public KeyboardMappingsCustomizationView()
        {
            InitializeComponent();

            //StyleHelper.CreateStyleForwardersForDefaultStyles(Application.Current.Resources, Resources);
        }
        #endregion

        private void OnNewInputGestureTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            var vm = ViewModel as KeyboardMappingsCustomizationViewModel;
            if (vm != null)
            {
                var modifiers = Keyboard.Modifiers;
                if (modifiers == ModifierKeys.Shift)
                {
                    return;
                }

                var key = e.Key;

                if (!IsValidKey(key))
                {
                    return;
                }

                var inputGesture = new InputGesture(key, modifiers);
                vm.SelectedCommandNewInputGesture = inputGesture;
            }
        }

        private bool IsValidKey(Key key)
        {
            if (key >= Key.A && key <= Key.Z)
            {
                return true;
            }

            return false;
        }
    }
}