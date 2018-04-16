// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageImageToTextConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Converters
{
    using System;
    using Catel.MVVM.Converters;
    using Catel.Services;

    internal class MessageImageToTextConverter : ValueConverterBase<MessageImage>
    {
        protected override object Convert(MessageImage value, Type targetType, object parameter)
        {
            return value.ToString();
        }
    }
}