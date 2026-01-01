namespace Orchestra.Examples.Ribbon.ViewModels
{
    using System;
    using Catel.MVVM;

    public class StatusBarViewModel : ViewModelBase
    {
        public StatusBarViewModel(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
        }

        public override string Title
        {
            get { return "Status bar title binding"; }
        }

        public bool EnableAutomaticUpdates { get; set; }
    }
}
