namespace Orchestra
{
    using System.Windows.Controls;

    public interface IProgressBarProvider
    {
        ProgressBar? GetProgressBar();
    }
}
