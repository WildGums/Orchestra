namespace Orchestra.Views
{
    using System.Windows.Input;
    using Catel;
    using Catel.IoC;
    using Catel.Windows.Controls;
    using Catel.Windows.Input;
    using Orchestra.Services;
    using ViewModels;
    using InputGesture = Catel.Windows.Input.InputGesture;

    /// <summary>
    /// Interaction logic for KeyboardMappingsView.xaml.
    /// </summary>
    public partial class KeyboardMappingsCustomizationView : UserControl
    {
        private readonly IKeyboardMappingsAllowedKeysService _keyboardMappingsAllowedKeysService;

        public KeyboardMappingsCustomizationView()
        {
            InitializeComponent();

            _keyboardMappingsAllowedKeysService = ServiceLocator.Default.ResolveRequiredType<IKeyboardMappingsAllowedKeysService>();
        }

        private void OnNewInputGestureTextBoxKeyDown(object? sender, KeyEventArgs e)
        {
            e.Handled = true;

            var vm = ViewModel as KeyboardMappingsCustomizationViewModel;
            if (vm is not null)
            {
                var modifiers = KeyboardHelper.GetCurrentlyPressedModifiers();
                if (modifiers.Count == 0 && modifiers[0] == ModifierKeys.Shift)
                {
                    // Only ignore just shift, control + shift is allowed
                    return;
                }

                var key = e.Key;

                if (!_keyboardMappingsAllowedKeysService.IsAllowed(key))
                {
                    return;
                }

                var finalModifiers = ModifierKeys.None;

                foreach (var modifier in modifiers)
                {
                    finalModifiers = Enum<ModifierKeys>.Flags.SetFlag(finalModifiers, modifier);
                }

                var inputGesture = new InputGesture(key, finalModifiers);
                vm.SelectedCommandNewInputGesture = inputGesture;
            }
        }
    }
}
