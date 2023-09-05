using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils.GUI;
using Sys = Cosmos.System;

namespace WinttOS.Base.GUI
{
    public class PowerOffButton : OSButton
    {
        [ManifestResourceStream(ResourceName = "WinttOS.Base.resources.button_power_off.bmp")]
        private static byte[] imageInBytes;
        public PowerOffButton(uint x, uint y) : base(x, y, new Bitmap(imageInBytes), true)
        { 
            
        }

        public override void onButtonPressed()
        {
            Sys.Power.Shutdown();
        }
    }
}
