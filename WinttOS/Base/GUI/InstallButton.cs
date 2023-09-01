using Cosmos.System.Graphics;
using System.Drawing;
using Image = Cosmos.System.Graphics.Image;
using Sys = Cosmos.System;
using WinttOS.Base.Utils.GUI;
using Bitmap = Cosmos.System.Graphics.Bitmap;

namespace WinttOS.Base.GUI
{
    public class InstallNextButton : OSButton
    {

        public InstallNextButton(uint x, uint y, Bitmap image) :
            base(x, y, image)
        { }

        public override void onButtonPressed()
        {
            uint mouseX = Cosmos.System.MouseManager.X;
            uint mouseY = Cosmos.System.MouseManager.Y;

            if (mouseX >= x && mouseX <= x + image.Width &&
                mouseY >= y && mouseY <= y + image.Height && 
                Sys.MouseManager.MouseState == Sys.MouseState.Left)
            {
                Sys.Power.Shutdown();
            }
        }
    }
}
