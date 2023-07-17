using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Base.Utils.GUI
{
    public class Install : UI
    {
        private Canvas canvas;
        private Bitmap bg;
        [ManifestResourceStream(ResourceName = "WinttOS.Base.resources.os_install_bg.bmp")]
        static byte[] bgBytes;
        public Install(Canvas canvas) 
        {
            bg = new Bitmap(bgBytes);
            this.canvas = canvas;
            this.canvas.DrawImage(bg, 0, 0);
        }

        public void UpdateWallpaper()
        {
            this.canvas.DrawImage(bg, 0, 0);
        }
    }
}
