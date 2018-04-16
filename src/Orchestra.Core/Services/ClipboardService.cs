// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClipboardService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


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