using Catel.IoC;
using Catel.Services;
using Catel.Services.Models;
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

        var languageService = serviceLocator.ResolveType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orchestra", "Orchestra.Properties", "Resources"));

        // TODO: Ensure dictionary is loaded?
        //<ResourceDictionary Source="/Catel.MVVM;component/themes/generic.xaml" />
    }
}