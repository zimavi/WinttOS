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
        protected readonly Canvas canvas;
        protected readonly int x;
        protected readonly int y;
        protected readonly int width;
        protected readonly int height;
        protected readonly Color color;
        protected readonly Bitmap image;

        public OSButton(Canvas canvas, int x, int y, int width, int height, Color color)
        {
            this.canvas = canvas;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.color = color;
        }

        public OSButton(Canvas canvas, int x, int y, Bitmap image)
        {
            this.canvas = canvas;
            this.x = x;
            this.y = y;
            this.image = image;
        }

        public bool isMouseOver(int mouseX, int mouseY)
        {
            if(image != null)
            {
                if(mouseX >= x && mouseX <= x + image.Width &&
                    mouseY >= y && mouseY <= y + image.Height)
                {
                    canvas.DrawImageAlpha(image, x, y);
                    return true;
                }

                canvas.DrawImageAlpha(image, x, y);
                return false;
            }
            else
            {
                if(mouseX >= x && mouseX <= x + width &&
                    mouseY >= y && mouseY <= y + height)
                {
                    canvas.DrawFilledRectangle(color, x, y, width, height);
                    return true;
                }

                canvas.DrawFilledRectangle(color, x, y, width, height);
                return false;
            }
        }
        public virtual void onButtonPressed(int mouseX, int mouseY) { }
    }
}
