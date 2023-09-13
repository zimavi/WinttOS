using Cosmos.System.Graphics;
using System.Drawing;
using Image = Cosmos.System.Graphics.Image;
using Sys = Cosmos.System;
using WinttOS.Core.Utils.GUI;
using Bitmap = Cosmos.System.Graphics.Bitmap;
using WinttOS.Core.Utils.Debugging;

namespace WinttOS.Core.GUI
{
    public class InstallNextButton : OSButton
    {

        public InstallNextButton(uint x, uint y, Bitmap image) :
            base(x, y, image, false)
        { }

        public override void onButtonPressed()
        {
            throw new System.NotImplementedException("Used not implemented button!");
        }
    }
}
