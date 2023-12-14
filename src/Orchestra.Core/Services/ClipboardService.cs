namespace Orchestra.Services
{
    using System.Windows;

    public class ClipboardService : IClipboardService
    {
        public void CopyToClipboard(string text)
        {
            Clipboard.SetText(text);
        }
    }
}
