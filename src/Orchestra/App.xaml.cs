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
            StyleHelper.CreateStyleForwardersForDefaultStyles(Current.Resources.MergedDictionaries[1]);

            var serviceLocator = ServiceLocator.Instance;
            Catel.Environment.RegisterDefaultViewModelServices();

            var viewLocator = serviceLocator.ResolveType<IViewLocator>();
            viewLocator.Register(typeof(ProgressNotifyableViewModel), typeof(Views.SplashScreen));

            var viewModelLocator = serviceLocator.ResolveType<IViewModelLocator>();
            viewModelLocator.Register(typeof(Views.SplashScreen), typeof(ProgressNotifyableViewModel));

            var bootstrapper = new OrchestraBootstrapper();
            bootstrapper.RunWithSplashScreen<ProgressNotifyableViewModel>();

            base.OnStartup(e);
        }
    }
}