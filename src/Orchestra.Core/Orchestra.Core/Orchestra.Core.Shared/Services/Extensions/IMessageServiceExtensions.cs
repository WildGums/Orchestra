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
            var buttons = string.Empty;

            switch (messageButton)
            {
                case MessageButton.OK:
                    buttons = "[ OK ]";
                    break;

                case MessageButton.OKCancel:
                    buttons = "[ OK ] | [ Cancel ]";
                    break;

                case MessageButton.YesNo:
                    buttons = "[ Yes ] | [ No ]";
                    break;

                case MessageButton.YesNoCancel:
                    buttons = "[ Yes ] | [ No ] | [ Cancel ]";
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return messageService.GetAsText(message, buttons);
        }

        public static string GetAsText(this IMessageService messageService, string message, string buttons)
        {
            Argument.IsNotNull(() => messageService);

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(message);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("--------------------------------------------");
            stringBuilder.AppendLine();
            stringBuilder.Append(buttons);

            return stringBuilder.ToString();
        }
    }
}