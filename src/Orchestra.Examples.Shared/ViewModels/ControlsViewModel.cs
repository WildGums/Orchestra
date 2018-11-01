namespace Orchestra.Examples.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;
    using Catel;
    using Catel.MVVM;
    using Catel.Reflection;
    using Orchestra.Examples.Services;

    public class ControlsViewModel : ViewModelBase
    {
        private IUpdatableAccentColorService _updatableAccentColorService;

        public ControlsViewModel(IUpdatableAccentColorService updatableAccentColorService)
        {
            Argument.IsNotNull(() => updatableAccentColorService);

            _updatableAccentColorService = updatableAccentColorService;

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
            _updatableAccentColorService.SetAccentColor(SelectedAccentColor);
        }
        #endregion
    }
}
