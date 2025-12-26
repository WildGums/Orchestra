namespace Orchestra
{
    using Catel.Services;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Core module which allows the registration of default services in the service collection.
    /// </summary>
    public static partial class OrchestraShellRibbonFluentModule
    {
        public static IServiceCollection AddOrchestraShellRibbonFluentServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IShellService, ShellService>();
            serviceCollection.AddSingleton<IShellRecoveryService, ShellRecoveryService>();
            serviceCollection.AddSingleton<IApplicationInitializationService, ApplicationInitializationServiceBase>();
            serviceCollection.AddSingleton<IShellConfigurationService, ShellConfigurationService>();
            serviceCollection.AddSingleton<IBusyIndicatorService, ProgressBusyIndicatorService>();
            serviceCollection.AddSingleton<IProgressBarProvider, ProgressBarProvider>();
            serviceCollection.AddSingleton<IXamlResourceService, XamlResourceService>();

            serviceCollection.AddSingleton<ThirdPartyNotice>((x) => new ResourceBasedThirdPartyNotice("Fluent.Ribbon", "https://github.com/fluentribbon/Fluent.Ribbon", "Orchestra.Shell.Ribbon.Fluent", "Orchestra", "Resources.ThirdPartyNotices.fluent.ribbon.txt"));

            serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orchestra.Shell.Ribbon.Fluent", "Orchestra.Properties", "Resources"));

            return serviceCollection;
        }
    }
}
