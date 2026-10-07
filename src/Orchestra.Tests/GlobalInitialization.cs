using System.Diagnostics;
using System.Globalization;
using Catel.Logging;
using Catel.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Orchestra.Tests;

[SetUpFixture]
public class GlobalInitialization
{
    [OneTimeSetUp]
    public static void SetUp()
    {
        LogManager.FallbackLoggerFactory = LoggerFactory.Create(x =>
        {
            if (Debugger.IsAttached)
            {
                x.AddFilter(x => x == LogLevel.Debug);

                x.AddDebug();
            }

            x.AddConsole();
        });

        var culture = new CultureInfo("en-US");
        System.Threading.Thread.CurrentThread.CurrentCulture = culture;
        System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

        // Required since we do multithreaded initialization
        TypeCache.InitializeTypes(allowMultithreadedInitialization: false);

        // Set a global service provider for helpers such as LanguageHelper
        var serviceCollection = ServiceCollectionHelper.CreateServiceCollection();

        Catel.IoC.IoCContainer.ServiceProvider = serviceCollection.BuildServiceProvider();
    }
}
