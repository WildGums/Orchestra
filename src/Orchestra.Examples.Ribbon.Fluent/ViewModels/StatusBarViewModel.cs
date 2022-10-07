namespace Orchestra.Examples.Ribbon.ViewModels
{
    using Catel.MVVM;

    public class StatusBarViewModel : ViewModelBase
    {
        public override string Title
        {
            get { return "Status bar title binding"; }
        }

        public bool EnableAutomaticUpdates { get; set; }
    }
}
