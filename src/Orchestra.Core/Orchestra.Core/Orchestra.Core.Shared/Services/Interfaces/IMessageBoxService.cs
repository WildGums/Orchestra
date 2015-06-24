namespace Orchestra.Services
{
    using System.Threading.Tasks;
    using Catel.Services;

    public interface IMessageBoxService
    {
        Task<MessageResult> Show(string message, string caption = "", MessageButton button = MessageButton.OK, MessageImage icon = MessageImage.None);

    }
}