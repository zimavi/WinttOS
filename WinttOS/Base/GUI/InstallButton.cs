using Cosmos.System.Graphics;
using System.Drawing;
using Image = Cosmos.System.Graphics.Image;
using Sys = Cosmos.System;
using WinttOS.Base.Utils.GUI;
using Bitmap = Cosmos.System.Graphics.Bitmap;

namespace WinttOS.Base.GUI
{
    public class InstallButton : OSButton
    {
        //public InstallButton(Canvas canvas, int x, int y, int width, int height, Color color) :
        //    base(canvas, x, y, width, height, color)
        //{ }

        public InstallButton(Canvas canvas, int x, int y, Bitmap image) :
            base(canvas, x, y, image)
        { }

        public override void onButtonPressed(int mouseX, int mouseY)
        {
            if(mouseX >= x && mouseX <= x + image.Width &&
                mouseY >= y && mouseY <= y + image.Height && 
                Sys.MouseManager.MouseState == Sys.MouseState.Left)
            {
                Sys.Power.Shutdown();
            }
        }
    }
}
