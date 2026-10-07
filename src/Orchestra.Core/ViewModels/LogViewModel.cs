namespace Orchestra.ViewModels;

using System;
using Catel.MVVM;

public partial class LogViewModel : ViewModelBase
{
    public LogViewModel(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        ValidateUsingDataAnnotations = false;
    }

    public override string Title
    {
        get { return "Log information"; }
    }
}
