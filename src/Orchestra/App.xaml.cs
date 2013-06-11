namespace Orchestra
{
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    using System.Windows.Markup;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.MVVM.ViewModels;
    using Catel.Windows;
    using Services;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            SetCurrentCulture();

            // Example of best performance options for Catel (but at the cost of validation features)
            //Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            //Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;
            //Catel.Data.ModelBase.SuspendValidationForAllModels = true;

            var serviceLocator = ServiceLocator.Default;
            Catel.Environment.RegisterDefaultViewModelServices();
            
            var viewLocator = serviceLocator.ResolveType<IViewLocator>();
            viewLocator.Register(typeof(ProgressNotifyableViewModel), typeof(Views.SplashScreen));

            var viewModelLocator = serviceLocator.ResolveType<IViewModelLocator>();
            viewModelLocator.Register(typeof(Views.SplashScreen), typeof(ProgressNotifyableViewModel));

            var bootstrapper = new OrchestraBootstrapper();

            StyleHelper.CreateStyleForwardersForDefaultStyles(Current.Resources.MergedDictionaries[1]);

            var configurationService = (IConfigurationService)ServiceLocator.Default.GetService(typeof(IConfigurationService));

            ConfigureShell(configurationService);

            bootstrapper.RunWithSplashScreen<ProgressNotifyableViewModel>();

            

            base.OnStartup(e);
        }

        /// <summary>
        /// Exemple on how to configure the shell.
        /// </summary>
        /// <param name="configurationService">The configuration service.</param>
        private static void ConfigureShell(IConfigurationService configurationService)
        {
            // Override the configarable items in the Shell by localized items.
            configurationService.Configuration.HelpTabText = Orchestra.Resources.Resources.HelpRibbonTabText;
            configurationService.Configuration.HelpGroupText = Orchestra.Resources.Resources.HelpGroupText;
            configurationService.Configuration.HelpButtonText = Orchestra.Resources.Resources.HelpButtonText;
        }

        private static void SetCurrentCulture()
        {
            ////Example culture, for testing purposes.
            //var culture = new CultureInfo("de-DE");

            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture;

            // Changes the Default WPF Culture (en-US), otherwise it will be used, instead of the system settings.
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof (FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }
    }
}