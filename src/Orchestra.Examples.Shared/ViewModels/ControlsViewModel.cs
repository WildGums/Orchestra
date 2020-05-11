namespace Orchestra.Examples.ViewModels
{
    using System.Collections.Generic;
    using Catel.Data;
    using Catel.MVVM;

    public class ControlsViewModel : ViewModelBase
    {
        public ControlsViewModel()
        {
        }

        #region Properties
        public string Text { get; set; }
        #endregion

        #region Methods
        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrEmpty(Text))
            {
                validationResults.Add(new FieldValidationResult(nameof(Text), ValidationResultType.Error, "Text cannot be empty"));
            }

            base.ValidateFields(validationResults);
        }
        #endregion
    }
}
