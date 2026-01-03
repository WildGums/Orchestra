namespace Orchestra
{
    using Catel.Services;
    using Catel.ThirdPartyNotices;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    /// Core module which allows the registration of default services in the service collection.
    /// </summary>
    public static partial class OrchestraShellRibbonFluentModule
    {
        public static IServiceCollection AddOrchestraShellRibbonFluent(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IShellService, ShellService>();
            serviceCollection.TryAddSingleton<IShellRecoveryService, ShellRecoveryService>();
            serviceCollection.TryAddSingleton<IApplicationInitializationService, ApplicationInitializationServiceBase>();
            serviceCollection.TryAddSingleton<IShellConfigurationService, ShellConfigurationService>();
            serviceCollection.TryAddSingleton<IBusyIndicatorService, ProgressBusyIndicatorService>();
            serviceCollection.TryAddSingleton<IProgressBarProvider, ProgressBarProvider>();
            serviceCollection.TryAddSingleton<IXamlResourceService, XamlResourceService>();

            serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new ResourceBasedThirdPartyNotice("Fluent.Ribbon", "https://github.com/fluentribbon/Fluent.Ribbon", "Orchestra.Shell.Ribbon.Fluent", "Orchestra", "Resources.ThirdPartyNotices.fluent.ribbon.txt"));

            serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orchestra.Shell.Ribbon.Fluent", "Orchestra.Properties", "Resources"));

            return serviceCollection;
        }
    }
}
