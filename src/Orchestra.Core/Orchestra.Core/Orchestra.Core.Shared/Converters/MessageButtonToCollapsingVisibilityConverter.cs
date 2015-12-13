// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageButtonToCollapsingVisibilityConverter.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Converters
{
    using System;
    using System.Windows;
    using Catel.Data;
    using Catel.MVVM.Converters;
    using Catel.Services;

    internal class MessageButtonToCollapsingVisibilityConverter : VisibilityConverterBase
    {
        public MessageButtonToCollapsingVisibilityConverter() 
            : base(Visibility.Collapsed)
        {
        }

        protected override bool IsVisible(object value, Type targetType, object parameter)
        {
            var button = (MessageButton)value;

            if (parameter.Equals("OK") && (button == MessageButton.OK || button == MessageButton.OKCancel))
            {
                return true;
            }
            if (parameter.Equals("Yes") && (button == MessageButton.YesNoCancel || button == MessageButton.YesNo))
            {
                return true;
            }
            if (parameter.Equals("No") && (button == MessageButton.YesNoCancel || button == MessageButton.YesNo))
            {
                return true;
            }
            if (parameter.Equals("Cancel") && (button == MessageButton.OKCancel || button == MessageButton.YesNoCancel))
            {
                return true;
            }

            return false;
        }

    }
}