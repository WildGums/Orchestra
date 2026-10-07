namespace Orchestra.Examples.ViewModels;

using System;
using System.Collections.Generic;
using Catel.Data;
using Catel.MVVM;

public partial class ControlsViewModel : FeaturedViewModelBase
{
    public ControlsViewModel(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }

    public string Text { get; set; }

    protected override void ValidateFields(List<IFieldValidationResult> validationResults)
    {
        if (string.IsNullOrEmpty(Text))
        {
            validationResults.Add(new FieldValidationResult(nameof(Text), ValidationResultType.Error, "Text cannot be empty"));
        }

        base.ValidateFields(validationResults);
    }
}
