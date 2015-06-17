// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressToVisibilityConverter.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Shell.Converters
{
    using System;
    using System.Windows;
    using Catel.MVVM.Converters;

    internal class ProgressToVisibilityConverter : VisibilityConverterBase

    {
        public ProgressToVisibilityConverter()
            : base(Visibility.Collapsed)
        {
            
        }

        protected override bool IsVisible(object value, Type targetType, object parameter)
        {
            var doubleValue = (double) value;

            if (doubleValue >= 0 && doubleValue <= 100)
            {
                return true;
            }

            return false;
        }
    }
}