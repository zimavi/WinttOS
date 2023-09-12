using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Base.Utils
{
    public static class Files
    {
        [ManifestResourceStream(ResourceName = "WinttOS.Base.resources.win7-startup.wav")]
        public static readonly byte[] RawStartupSound;

        [ManifestResourceStream(ResourceName = "WinttOS.Base.resources.cur.bmp")]
        public static readonly byte[] RawCursorImage;

        [ManifestResourceStream(ResourceName = "WinttOS.Base.resources.button_power_off.bmp")]
        public static readonly byte[] RawPowerOffButtonImageAlpha;

        public static class Installer
        {
            [ManifestResourceStream(ResourceName = "WinttOS.Base.resources.os_install_bg.bmp")]
            public static readonly byte[] RawInstallerBackgroundImage;

            [ManifestResourceStream(ResourceName = "WinttOS.Base.resources.os_next_inst_img.bmp")]
            public static readonly byte[] RawInstallerNextButtonImage;
        }
    }
}
