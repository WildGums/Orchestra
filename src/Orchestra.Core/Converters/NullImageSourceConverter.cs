namespace Orchestra.Converters
{
    using System;
    using System.Windows;
    using Catel.MVVM.Converters;

    public class NullImageSourceConverter : ValueConverterBase
    {
        protected override object? Convert(object? value, Type targetType, object? parameter)
        {
            if (value is null)
            {
                return DependencyProperty.UnsetValue;
            }

            return value;
        }
    }
}
