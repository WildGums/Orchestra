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
        private readonly IBaseColorService _baseColorService;

        public ControlsViewModel(Orc.Controls.Services.IAccentColorService accentColorService, IBaseColorService baseColorService)
        {
            Argument.IsNotNull(() => accentColorService);
            Argument.IsNotNull(() => baseColorService);

            _accentColorService = accentColorService;
            _baseColorService = baseColorService;

            AccentColors = typeof(Colors).GetPropertiesEx(true, true).Where(x => x.PropertyType.IsAssignableFromEx(typeof(Color))).Select(x => (Color)x.GetValue(null)).ToList();
            SelectedAccentColor = Colors.Orange;

            BaseColors = _baseColorService.GetAvailableBaseColors();
            SelectedBaseColor = BaseColors[0];
        }

        #region
        public List<Color> AccentColors { get; private set; }
        public IReadOnlyList<string> BaseColors { get; private set; }

        public Color SelectedAccentColor { get; set; }
        public string SelectedBaseColor { get; set; }
        #endregion

        #region Methods
        private void OnSelectedAccentColorChanged()
        {
            _accentColorService.SetAccentColor(SelectedAccentColor);
        }
        private void OnSelectedBaseColorChanged()
        {
            _baseColorService.SetBaseColor(SelectedBaseColor);
        }
        #endregion
    }
}
