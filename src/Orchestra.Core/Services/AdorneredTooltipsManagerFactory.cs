namespace Orchestra;

using System;
using System.Windows.Documents;
using Catel.IoC;
using Layers;
using Microsoft.Extensions.DependencyInjection;
using Tooltips;

public class AdorneredTooltipsManagerFactory : IAdorneredTooltipsManagerFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IAdorneredTooltipFactory _adorneredTooltipFactory;
    private readonly IAdornerTooltipGenerator _adornerTooltipGenerator;
    private readonly IHintsProvider _hintsProvider;

    public AdorneredTooltipsManagerFactory(IServiceProvider serviceProvider,
        IAdorneredTooltipFactory adorneredTooltipFactory, IAdornerTooltipGenerator adornerTooltipGenerator,
        IHintsProvider hintsProvider)
    {
        _serviceProvider = serviceProvider;
        _adorneredTooltipFactory = adorneredTooltipFactory;
        _adornerTooltipGenerator = adornerTooltipGenerator;
        _hintsProvider = hintsProvider;
    }

    public IAdorneredTooltipsManager Create(AdornerLayer adornerLayer)
    {
        ArgumentNullException.ThrowIfNull(adornerLayer);

        var hintsAdornerLayer = ActivatorUtilities.CreateInstance<IAdornerLayer>(_serviceProvider, new object[] { adornerLayer });
        var adorneredHintsCollection = ActivatorUtilities.CreateInstance<IAdorneredTooltipFactory>(_serviceProvider, new object[] { _adorneredTooltipFactory });

        return ActivatorUtilities.CreateInstance<AdorneredTooltipsManager>(_serviceProvider, new object[] { _adornerTooltipGenerator, _hintsProvider, hintsAdornerLayer, adorneredHintsCollection });
    }
}
