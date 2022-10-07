namespace Orchestra.Behaviors
{
    using System;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;
    using Catel.Logging;
    using Catel.Windows.Input;
    using Catel.Windows.Interactivity;
    using Services;

    public class HintsBehavior : BehaviorBase<FrameworkElement>
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IAdorneredTooltipsManagerFactory _adorneredTooltipsManagerFactory;

        public HintsBehavior(IAdorneredTooltipsManagerFactory adorneredTooltipsManagerFactory)
        {
            ArgumentNullException.ThrowIfNull(adorneredTooltipsManagerFactory);

            _adorneredTooltipsManagerFactory = adorneredTooltipsManagerFactory;
        }

        public Visual? Adorner
        {
            get { return (Visual?)GetValue(AdornerProperty); }
            set { SetValue(AdornerProperty, value); }
        }

        public static readonly DependencyProperty AdornerProperty = DependencyProperty.Register(nameof(Adorner), typeof(Visual), 
            typeof(HintsBehavior), new PropertyMetadata(null));


        protected override void Initialize()
        {
            base.Initialize();

            var adornerLayer = AdornerLayer.GetAdornerLayer(Adorner ?? AssociatedObject);
            if (adornerLayer is null)
            {
                Log.Error("Cannot find AdornerLayer. Use the Adorner property to specify a specific instance to use when searching for an adorner layer");
                return;
            }

            if (KeyboardHelper.AreKeyboardModifiersPressed(System.Windows.Input.ModifierKeys.Alt))
            {
                var adorneredTooltipsManager = _adorneredTooltipsManagerFactory.Create(adornerLayer);
                adorneredTooltipsManager.Enable();
            }
        }
    }
}
