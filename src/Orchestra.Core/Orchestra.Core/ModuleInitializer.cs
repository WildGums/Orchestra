using Catel.IoC;
using Catel.Services;
using Orchestra;
using Orchestra.Services;
using Orchestra.ViewModels;
using Orchestra.Views;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterTypeIfNotYetRegistered<IRecentlyUsedItemsService, RecentlyUsedItemsesService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IKeyboardMappingsService, KeyboardMappingsService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IStatusService, StatusService>();

        var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();
        uiVisualizerService.Register(typeof(KeyboardMappingsCustomizationViewModel), typeof(KeyboardMappingsCustomizationWindow));
        uiVisualizerService.Register(typeof(KeyboardMappingsOverviewViewModel), typeof(KeyboardMappingsOverviewWindow));

        DotNetPatchHelper.Initialize();
    }
}