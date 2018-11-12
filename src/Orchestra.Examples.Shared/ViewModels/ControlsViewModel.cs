namespace Orchestra.Examples.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;
    using Catel;
    using Catel.MVVM;
    using Catel.Reflection;
    using Orc.Controls.Services;

    public class ControlsViewModel : ViewModelBase
    {
        private IAccentColorService _accentColorService;

        public ControlsViewModel(IAccentColorService accentColorService)
        {
            Argument.IsNotNull(() => accentColorService);

            _accentColorService = accentColorService;

            AccentColors = typeof(Colors).GetPropertiesEx(true, true).Where(x => x.PropertyType.IsAssignableFromEx(typeof(Color))).Select(x => (Color)x.GetValue(null)).ToList();
            SelectedAccentColor = Colors.Orange;
        }

        #region
        public List<Color> AccentColors { get; private set; }

        public Color SelectedAccentColor { get; set; }
        #endregion

        #region Methods
        private void OnSelectedAccentColorChanged()
        {
            _accentColorService.SetAccentColor(SelectedAccentColor);
        }
        #endregion
    }
}
