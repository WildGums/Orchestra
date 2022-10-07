namespace Orchestra.Services
{
    using System;
    using System.Text;
    using Catel.Services;

    public static class IMessageServiceExtensions
    {
        public static string GetAsText(this IMessageService messageService, string message, MessageButton messageButton)
        {
            ArgumentNullException.ThrowIfNull(messageService);
            ArgumentNullException.ThrowIfNull(message);

            string buttons;

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
                    throw new ArgumentOutOfRangeException(nameof(messageButton));
            }

            return messageService.GetAsText(message, buttons);
        }

        public static string GetAsText(this IMessageService messageService, string message, string buttons)
        {
            ArgumentNullException.ThrowIfNull(messageService);
            ArgumentNullException.ThrowIfNull(message);

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
