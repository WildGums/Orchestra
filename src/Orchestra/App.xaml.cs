namespace Orchestra
{
    using System.Windows;

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
            StyleHelper.CreateStyleForwardersForDefaultStyles(Application.Current.Resources.MergedDictionaries[1]);

            var bootstrapper = new OrchestraBootstrapper();
            bootstrapper.Run();

            base.OnStartup(e);
        }
    }
}