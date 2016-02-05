// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanToThicknessConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Converters
{
    using System;
    using System.Windows;
    using Catel.MVVM.Converters;

    public class BooleanToThicknessConverter : ValueConverterBase<bool>
    {
        protected override object Convert(bool value, Type targetType, object parameter)
        {
            var thickness = value ? 1d : 0d;

            return new Thickness(thickness);
        }
    }
}