// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HintsBehavior.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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
    using Catel.Windows.Interactivity;
    using Services;
    using Tooltips;

    public class HintsBehavior : BehaviorBase<FrameworkElement>
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IAdorneredTooltipsManagerFactory _adorneredTooltipsManagerFactory;
        private readonly IAdorneredTooltipFactory _adorneredTooltipFactory;
        private readonly IHintsProvider _hintsProvider;
        private readonly IAdornerTooltipGenerator _adornerTooltipGenerator;

        private IAdorneredTooltipsManager _adorneredTooltipsManager;

        public HintsBehavior(IAdorneredTooltipsManagerFactory adorneredTooltipsManagerFactory, IAdorneredTooltipFactory adorneredTooltipFactory,
            IHintsProvider hintsProvider, IAdornerTooltipGenerator adornerTooltipGenerator)
        {
            Argument.IsNotNull(() => adorneredTooltipsManagerFactory);
            Argument.IsNotNull(() => adorneredTooltipFactory);
            Argument.IsNotNull(() => hintsProvider);
            Argument.IsNotNull(() => adornerTooltipGenerator);

            _adorneredTooltipsManagerFactory = adorneredTooltipsManagerFactory;
            _adorneredTooltipFactory = adorneredTooltipFactory;
            _hintsProvider = hintsProvider;
            _adornerTooltipGenerator = adornerTooltipGenerator;
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

            _adorneredTooltipsManager = _adorneredTooltipsManagerFactory.Create(adornerLayer);

            // TODO: Only enable on ALT key
            _adorneredTooltipsManager.Enable();
        }
    }
}