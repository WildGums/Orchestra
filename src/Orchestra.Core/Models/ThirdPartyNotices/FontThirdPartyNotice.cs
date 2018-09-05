namespace Orchestra
{
    using Catel;

    public class FontThirdPartyNotice : ThirdPartyNotice
    {
        public FontThirdPartyNotice(string fontName, string fontUrl)
        {
            Argument.IsNotNull(() => fontName);

            Title = fontName;
            Url = fontUrl;
        }
    }
}
