using Cosmos.System;
using System.Drawing;
using System.Diagnostics;
using Graph = Cosmos.System.Graphics;
using Cosmos.System.Graphics;
using Bitmap = Cosmos.System.Graphics.Bitmap;

namespace WinttOS.Base.Utils.GUI
{
    public abstract class OSButton
    {
        public readonly uint x;
        public readonly uint y;
        public readonly uint width;
        public readonly uint height;
        public readonly Color color;
        public readonly Bitmap image;
        private readonly bool usingImage;

        public OSButton(uint x, uint y, uint width, uint height, Color color)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.color = color;
            usingImage = false;
        }

        public OSButton(uint x, uint y, Bitmap image)
        {
            this.x = x;
            this.y = y;
            this.image = image;
            this.height = image.Height;
            this.width = image.Width;
            usingImage = true;
        }

        public bool isMouseOver(Canvas canvas)
        {

            uint mouseX = Cosmos.System.MouseManager.X;
            uint mouseY = Cosmos.System.MouseManager.Y;

            if (usingImage)
            {
                //canvas.DrawImageAlpha(image, (int)x, (int)y);
                return mouseX >= x && mouseX <= x + image.Width && mouseY >= y && mouseY <= y + image.Height;
            }
            else
            {
                //canvas.DrawFilledRectangle(color, (int)x, (int)y, (int)width, (int)height);
                return mouseX >= x && mouseX <= x + width && mouseY >= y && mouseY <= y + height;
            }
        }
        public virtual void onButtonPressed() { }
    }
}
