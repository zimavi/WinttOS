using Cosmos.System.Graphics;
using WinttOS.Core.Utils.GUI;
using WinttOS.Core.Utils.System;
using Sys = Cosmos.System;

namespace WinttOS.System.GUI
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
