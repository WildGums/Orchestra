namespace Orchestra
{
    using System.Windows;
    using System.Windows.Controls;

    public class ProgressBarProvider : IProgressBarProvider
    {
        public virtual ProgressBar? GetProgressBar()
        {
            return Application.Current.MainWindow?.FindName("pleaseWaitProgressBar") as ProgressBar;
        }
    }
}
