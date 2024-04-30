using Cosmos.System.Graphics.Fonts;
using IL2CPU.API.Attribs;

namespace WinttOS.Core.Utils.Sys
{
    public static class Files
    {
        [ManifestResourceStream(ResourceName = "WinttOS.Core.resources.startup.wav")]
        public static readonly byte[] RawStartupSound;

        [ManifestResourceStream(ResourceName = "WinttOS.Core.resources.button_power_off.bmp")]
        public static readonly byte[] RawPowerOffButtonImageAlpha;

        [ManifestResourceStream(ResourceName = "WinttOS.Core.resources.zap-ext-light18.psf")]
        public static byte[] RawFont18;

        public static class Fonts
        {
            public static PCScreenFont Font18 = PCScreenFont.LoadFont(RawFont18);
        }
    }
}
