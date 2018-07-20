// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UrlInfo.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using Catel;

    public class UriInfo
    {
        public UriInfo(string uri, string displayText = null)
        {
            Argument.IsNotNull(() => uri);

            Uri = uri;
            DisplayText = displayText ?? uri;
        }

        public string DisplayText { get; set; }

        public string Uri { get; set; }
    }
}