namespace Orchestra.Converters
{
    using System;
    using System.Windows;
    using Catel.MVVM.Converters;

    public class BooleanToThicknessConverter : ValueConverterBase<bool>
    {
        protected override object? Convert(bool value, Type targetType, object? parameter)
        {
            var thickness = value ? 1d : 0d;

            return new Thickness(thickness);
        }
    }
}
