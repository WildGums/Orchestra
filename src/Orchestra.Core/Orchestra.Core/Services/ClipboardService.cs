// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClipboardService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2015 Orchestra development team. All rights reserved.
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