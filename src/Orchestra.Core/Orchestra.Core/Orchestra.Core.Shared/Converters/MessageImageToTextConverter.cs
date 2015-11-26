// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageImageToTextConverter.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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
            switch (value)
            {
                case MessageImage.Error:
                    return MessageImage.Error.ToString();

                case MessageImage.Information:
                    return MessageImage.Information.ToString();

                case MessageImage.Warning:
                    return MessageImage.Warning.ToString();

                default:
                    return string.Empty;
            }
        }
    }
}