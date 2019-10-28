namespace Orchestra.Examples.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;
    using Catel;
    using Catel.MVVM;
    using Catel.Reflection;
    using Orc.Controls.Services;
    using Orchestra.Services;

    public class ControlsViewModel : ViewModelBase
    {
        private readonly Orc.Controls.Services.IAccentColorService _accentColorService;
        private readonly IBaseColorSchemeService _baseColorSchemeService;

        public ControlsViewModel(Orc.Controls.Services.IAccentColorService accentColorService, IBaseColorSchemeService baseColorSchemeService)
        {
            Argument.IsNotNull(() => accentColorService);
            Argument.IsNotNull(() => baseColorSchemeService);

            _accentColorService = accentColorService;
            _baseColorSchemeService = baseColorSchemeService;

            AccentColors = typeof(Colors).GetPropertiesEx(true, true).Where(x => x.PropertyType.IsAssignableFromEx(typeof(Color))).Select(x => (Color)x.GetValue(null)).ToList();
            SelectedAccentColor = Colors.Orange;

            BaseColorSchemes = _baseColorSchemeService.GetAvailableBaseColorSchemes();
            SelectedBaseColorScheme = BaseColorSchemes[0];
        }

        #region
        public List<Color> AccentColors { get; private set; }
        public IReadOnlyList<string> BaseColorSchemes { get; private set; }

        public Color SelectedAccentColor { get; set; }
        public string SelectedBaseColorScheme { get; set; }
        #endregion

        #region Methods
        private void OnSelectedAccentColorChanged()
        {
            _accentColorService.SetAccentColor(SelectedAccentColor);
        }

        private void OnSelectedBaseColorSchemeChanged()
        {
            _baseColorSchemeService.SetBaseColorScheme(SelectedBaseColorScheme);
        }
        #endregion
    }
}
