using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Core.Utils.System
{
    public static class Files
    {
        [ManifestResourceStream(ResourceName = "WinttOS.Core.resources.startup.wav")]
        public static readonly byte[] RawStartupSound;

        [ManifestResourceStream(ResourceName = "WinttOS.Core.resources.cur.bmp")]
        public static readonly byte[] RawCursorImage;

        [ManifestResourceStream(ResourceName = "WinttOS.Core.resources.button_power_off.bmp")]
        public static readonly byte[] RawPowerOffButtonImageAlpha;

        [ManifestResourceStream(ResourceName = "WinttOS.Core.resources.zap-ext-light18.psf")]
        public static readonly byte[] RawVGAFont;

        public static class Installer
        {
            [ManifestResourceStream(ResourceName = "WinttOS.Core.resources.os_install_bg.bmp")]
            public static readonly byte[] RawInstallerBackgroundImage;

            [ManifestResourceStream(ResourceName = "WinttOS.Core.resources.os_next_inst_img.bmp")]
            public static readonly byte[] RawInstallerNextButtonImage;
        }
    }
}
