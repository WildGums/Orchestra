namespace Orchestra.Win32
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal enum VideoOutputTechnology : uint
    {
        DisplayConfigOutputTechnologyOther = 0xFFFFFFFF,
        DisplayConfigOutputTechnologyHd15 = 0,
        DisplayConfigOutputTechnologySvideo = 1,
        DisplayConfigOutputTechnologyCompositeVideo = 2,
        DisplayConfigOutputTechnologyComponentVideo = 3,
        DisplayConfigOutputTechnologyDvi = 4,
        DisplayConfigOutputTechnologyHdmi = 5,
        DisplayConfigOutputTechnologyLvds = 6,
        DisplayConfigOutputTechnologyDJpn = 8,
        DisplayConfigOutputTechnologySdi = 9,
        DisplayconfigOutputTechnologyDisplayportExternal = 10,
        DisplayConfigOutputTechnologyDisplayportEmbedded = 11,
        DisplayConfigOutputTechnologyUdiExternal = 12,
        DisplayConfigOutputTechnologyUdiEmbedded = 13,
        DisplayConfigOutputTechnologySdtvdongle = 14,
        DisplayConfigOutputTechnologyMiracast = 15,
        DisplayConfigOutputTechnologyInternal = 0x80000000,
        DisplayConfigOutputTechnologyForceUint32 = 0xFFFFFFFF
    }
}
