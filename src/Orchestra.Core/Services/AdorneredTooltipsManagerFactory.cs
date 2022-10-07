namespace Orchestra.Services
{
    using System;
    using System.Windows.Documents;
    using Catel.IoC;
    using Layers;
    using Tooltips;

    public class AdorneredTooltipsManagerFactory : IAdorneredTooltipsManagerFactory
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly ITypeFactory _typeFactory;

        public AdorneredTooltipsManagerFactory(IServiceLocator serviceLocator, ITypeFactory typeFactory)
        {
            ArgumentNullException.ThrowIfNull(serviceLocator);
            ArgumentNullException.ThrowIfNull(typeFactory);

            _serviceLocator = serviceLocator;
            _typeFactory = typeFactory;
        }

        public IAdorneredTooltipsManager Create(AdornerLayer adornerLayer)
        {
            ArgumentNullException.ThrowIfNull(adornerLayer);

            var hintsAdornerLayer = _serviceLocator.ResolveRequiredTypeUsingParameters<IAdornerLayer>(new object[] { adornerLayer });
            var adorneredHintFactory = _serviceLocator.ResolveRequiredType<IAdorneredTooltipFactory>();
            var adorneredHintsCollection = _serviceLocator.ResolveRequiredTypeUsingParameters<IAdorneredTooltipFactory>(new object[] { adorneredHintFactory });

            var adornerGenerator = _serviceLocator.ResolveRequiredType<IAdornerTooltipGenerator>();
            var hintsProvider = _serviceLocator.ResolveRequiredType<IHintsProvider>();

            return _typeFactory.CreateRequiredInstanceWithParameters<AdorneredTooltipsManager>(adornerGenerator, hintsProvider, hintsAdornerLayer, adorneredHintsCollection);
        }
    }
}
