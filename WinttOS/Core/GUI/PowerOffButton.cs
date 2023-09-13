using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils;
using WinttOS.Core.Utils.GUI;
using Sys = Cosmos.System;

namespace WinttOS.Core.GUI
{
    public class PowerOffButton : OSButton
    {
        public PowerOffButton(uint x, uint y) : base(x, y, new Bitmap(Files.RawPowerOffButtonImageAlpha), true)
        { 
            
        }

        public override void onButtonPressed()
        {
            Sys.Power.Shutdown();
        }
    }
}
