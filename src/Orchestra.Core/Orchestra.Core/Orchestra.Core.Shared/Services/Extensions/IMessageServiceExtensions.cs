// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMessageServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Text;
    using System.Windows.Forms;
    using Catel;
    using Catel.Services;

    public static class IMessageServiceExtensions
    {
        public static string GetAsText(this IMessageService messageService, string message, MessageButton messageButton)
        {
            Argument.IsNotNull(() => messageService);

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(message);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("--------------------------------------------");
            stringBuilder.AppendLine();

            switch (messageButton)
            {
                case MessageButton.OK:
                    stringBuilder.Append("[ OK ]");
                    break;

                case MessageButton.OKCancel:
                    stringBuilder.Append("[ OK ] | [ Cancel ]");
                    break;

                case MessageButton.YesNo:
                    stringBuilder.Append("[ Yes ] | [ No ]");
                    break;

                case MessageButton.YesNoCancel:
                    stringBuilder.Append("[ Yes ] | [ No ] | [ Cancel ]");
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return stringBuilder.ToString();
        }
    }
}