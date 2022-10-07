namespace Orchestra.Converters
{
    using System;
    using System.Windows;
    using Catel.MVVM.Converters;
    using Catel.Services;

    internal class MessageButtonToCollapsingVisibilityConverter : VisibilityConverterBase
    {
        public MessageButtonToCollapsingVisibilityConverter()
            : base(Visibility.Collapsed)
        {
            SupportInversionUsingCommandParameter = false;
        }

        protected override bool IsVisible(object? value, Type targetType, object? parameter)
        {
            var button = value as MessageButton?;
            if (button is null)
            {
                return false;
            }

            var stringParameter = parameter as string;
            if (stringParameter is null)
            {
                return false;
            }


            if (stringParameter.Equals("OK") && (button == MessageButton.OK || button == MessageButton.OKCancel))
            {
                return true;
            }

            if (stringParameter.Equals("Yes") && (button == MessageButton.YesNoCancel || button == MessageButton.YesNo))
            {
                return true;
            }

            if (stringParameter.Equals("No") && (button == MessageButton.YesNoCancel || button == MessageButton.YesNo))
            {
                return true;
            }

            return stringParameter.Equals("Cancel") && (button == MessageButton.OKCancel || button == MessageButton.YesNoCancel);
        }
    }
}
