// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdorneredTooltipsManagerFactory.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Windows.Documents;
    using Catel;
    using Catel.IoC;
    using Layers;
    using Tooltips;

    public class AdorneredTooltipsManagerFactory : IAdorneredTooltipsManagerFactory
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly ITypeFactory _typeFactory;

        public AdorneredTooltipsManagerFactory(IServiceLocator serviceLocator, ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => serviceLocator);
            Argument.IsNotNull(() => typeFactory);

            _serviceLocator = serviceLocator;
            _typeFactory = typeFactory;
        }

        #region Methods
        public IAdorneredTooltipsManager Create(AdornerLayer adornerLayer)
        {
            Argument.IsNotNull(() => adornerLayer);

            var hintsAdornerLayer = _serviceLocator.ResolveTypeUsingParameters<IAdornerLayer>(new object[] { adornerLayer });
            var adorneredHintFactory = _serviceLocator.ResolveType<IAdorneredTooltipFactory>();
            var adorneredHintsCollection = _serviceLocator.ResolveTypeUsingParameters<IAdorneredTooltipFactory>(new object[] { adorneredHintFactory });

            var adornerGenerator = _serviceLocator.ResolveType<IAdornerTooltipGenerator>();
            var hintsProvider = _serviceLocator.ResolveType<IHintsProvider>();

            return _typeFactory.CreateInstanceWithParameters<AdorneredTooltipsManager>(adornerGenerator, hintsProvider, hintsAdornerLayer, adorneredHintsCollection);
        }
        #endregion
    }
}