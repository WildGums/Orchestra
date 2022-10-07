namespace Orchestra
{
    using System;
    
    public class UriInfo
    {
        public UriInfo(string uri, string? displayText = null)
        {
            ArgumentNullException.ThrowIfNull(uri);

            Uri = uri;
            DisplayText = displayText ?? uri;
        }

        public string DisplayText { get; set; }

        public string Uri { get; set; }
    }
}
