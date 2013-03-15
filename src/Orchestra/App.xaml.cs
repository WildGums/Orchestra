namespace Orchestra
{
    using System.Windows;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.MVVM.ViewModels;
    using Catel.Windows;

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

            bootstrapper.RunWithSplashScreen<ProgressNotifyableViewModel>();

            base.OnStartup(e);
        }
    }
}