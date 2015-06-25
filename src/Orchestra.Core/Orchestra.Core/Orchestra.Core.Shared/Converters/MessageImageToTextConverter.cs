using System;
using System.Collections.Generic;
using System.Text;

namespace Orchestra.Converters
{
    using Catel.MVVM.Converters;
    using Catel.Services;

    class MessageImageToTextConverter :ValueConverterBase<MessageImage>
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
