using Cosmos.System.Graphics.Fonts;
using IL2CPU.API.Attribs;

namespace WinttOS.Core.Utils.Sys
{
    public static class Files
    {
        [ManifestResourceStream(ResourceName = "WinttOS.Core.Embaded.zap_light18.psf")]
        public static byte[] RawBackup18;

        public static class Fonts
        {
            public static PCScreenFont Font18;
            public static PCScreenFont Backup18 = PCScreenFont.LoadFont(RawBackup18);
        }
    }
}
