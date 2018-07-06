// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HintsBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Behaviors
{
    using System;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Windows.Input;
    using Catel.Windows.Interactivity;
    using Services;
    using Tooltips;

    public class HintsBehavior : BehaviorBase<FrameworkElement>
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IAdorneredTooltipsManagerFactory _adorneredTooltipsManagerFactory;

        public HintsBehavior(IAdorneredTooltipsManagerFactory adorneredTooltipsManagerFactory)
        {
            Argument.IsNotNull(() => adorneredTooltipsManagerFactory);

            _adorneredTooltipsManagerFactory = adorneredTooltipsManagerFactory;
        }

        #region Properties
        public Visual Adorner
        {
            get { return (Visual)GetValue(AdornerProperty); }
            set { SetValue(AdornerProperty, value); }
        }

        public static readonly DependencyProperty AdornerProperty = DependencyProperty.Register("Adorner", typeof(Visual), 
            typeof(HintsBehavior), new PropertyMetadata(null));
        #endregion


        protected override void Initialize()
        {
            base.Initialize();

            var adornerLayer = AdornerLayer.GetAdornerLayer(Adorner ?? AssociatedObject);
            if (adornerLayer == null)
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
