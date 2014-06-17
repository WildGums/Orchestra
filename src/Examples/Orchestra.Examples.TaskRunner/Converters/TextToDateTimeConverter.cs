// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextToDateTimeConverter.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.TaskRunner.Converters
{
    using System;
    using Catel.MVVM.Converters;

    public class TextToDateTimeConverter : ValueConverterBase
    {
        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (!(value is DateTime))
            {
                return string.Empty;
            }

            var dateTime = (DateTime) value;
            return dateTime.ToString();
        }

        protected override object ConvertBack(object value, Type targetType, object parameter)
        {
            var dateTime = DateTime.Now;
            if (DateTime.TryParse(value as string, out dateTime))
            {
                return dateTime;
            }

            return null;
        }
        #endregion
    }
}