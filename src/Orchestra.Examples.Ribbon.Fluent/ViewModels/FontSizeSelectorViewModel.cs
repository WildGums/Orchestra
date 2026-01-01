namespace Orchestra.Examples.Ribbon.ViewModels
{
    using System;
    using Catel.MVVM;

    public class FontSizeSelectorViewModel : ViewModelBase
    {
        public FontSizeSelectorViewModel(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Title = "Please select the base font size";            
        }
    }
}
