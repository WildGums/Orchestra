namespace Orchestra.Examples.Ribbon;

using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Catel;
using Catel.IoC;
using Catel.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orc;
using Orchestra.Changelog;
using Orchestra.Examples.Ribbon.Services;
using Orchestra.Logging;
using Orchestra.Views;
using Serilog;
using Serilog.Core;

public partial class App : Application
{
#pragma warning disable IDISP006 // Implement IDisposable
    private readonly IHost _host;
#pragma warning restore IDISP006 // Implement IDisposable

    public App()
    {
        var hostBuilder = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                // Logging
                services.AddLogging(x =>
                {
                    x.AddSerilog();
                });

                services.AddKeyedSingleton("logging", (sp, k) => new InitializeAtStartup(() =>
                {
                    var logDirectoryProvider = sp.GetRequiredService<LogDirectoryProvider>();

#pragma warning disable IDISP003 // Dispose previous before re-assigning
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .MinimumLevel.Debug()
                        .WriteTo.File(Path.Combine(logDirectoryProvider.ProvideDirectory(), "Application-.log"), 
                            fileSizeLimitBytes: 25 * 1000 * 1024, // 25 MB
                            rollingInterval: RollingInterval.Hour,
                            rollOnFileSizeLimit: true,
                            levelSwitch: new LoggingLevelSwitch(Serilog.Events.LogEventLevel.Debug))
#if DEBUG
                        .WriteTo.Debug()
#endif
                        .CreateLogger();
#pragma warning restore IDISP003 // Dispose previous before re-assigning

                    var logger = sp.GetRequiredService<ILogger<App>>();
                    logger.LogApplicationInfo<App>();
                }));


                services.AddKeyedSingleton("a", (sp, k) => new InitializeAtStartup(() =>
                {
                    Console.WriteLine("A");
                }));

                services.AddKeyedSingleton("b", (sp, k) => new InitializeAtStartup(() =>
                {
                    Console.WriteLine("B");
                }));

                // Service registration
                services.AddCatelCore();
                services.AddCatelMvvm();
                services.AddOrcAutomation();
                services.AddOrcControls();
                services.AddOrcFileSystem();
                services.AddOrcLogViewer();
                services.AddOrcNotifications();
                services.AddOrcSerializationJson();
                services.AddOrcSystemInfo();
                services.AddOrcTheming();
                services.AddOrchestraCore();
                services.AddOrchestraShellRibbonFluent();

                services.AddSingleton<IAboutInfoService, AboutInfoService>();
                services.AddSingleton<IRibbonService, RibbonService>();
                services.AddSingleton<IApplicationInitializationService, ApplicationInitializationService>();

                services.AddSingleton<UserMessageCloseApplicationWatcher>();

                services.AddSingleton<IChangelogProvider, Orchestra.Examples.Ribbon.Changelog.Providers.ChangelogProvider>();
            });

        _host = hostBuilder.Build();

        IoCContainer.ServiceProvider = _host.Services;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var serviceProvider = IoCContainer.ServiceProvider;

        serviceProvider.CreateTypesThatMustBeConstructedAtStartup();

        var languageService = serviceProvider.GetRequiredService<ILanguageService>();

        // Note: it's best to use .CurrentUICulture in actual apps since it will use the preferred language
        // of the user. But in order to demo multilingual features for devs (who mostly have en-US as .CurrentUICulture),
        // we use .CurrentCulture for the sake of the demo
        languageService.PreferredCulture = CultureInfo.CurrentCulture;
        languageService.FallbackCulture = new CultureInfo("en-US");

        Orc.Theming.FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/Orchestra.Examples.Ribbon.Fluent;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));
        Orc.Theming.FontImage.DefaultFontFamily = "FontAwesome";

        var shellService = serviceProvider.GetRequiredService<IShellService>();
        shellService.CreateAsync<ShellWindow>();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync();
        }

        base.OnExit(e);
    }
}
