// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UrlInfo.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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