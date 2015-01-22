// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IClipboardService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2015 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    public interface IClipboardService
    {
        void CopyToClipboard(string text);
    }
}