namespace Orchestra
{
    using Catel;

    public class FontThirdPartyNotice : ThirdPartyNotice
    {
        public FontThirdPartyNotice(string fontName, string fontUrl)
        {
            ArgumentNullException.ThrowIfNull(fontName);

            Title = fontName;
            Url = fontUrl;
        }
    }
}
