using Cosmos.System;
using System.Drawing;
using System.Diagnostics;
using Graph = Cosmos.System.Graphics;
using Cosmos.System.Graphics;
using Bitmap = Cosmos.System.Graphics.Bitmap;
using System.Threading;

namespace WinttOS.Core.Utils.GUI
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
        public readonly bool imageHasAlpha;
        private bool isMouseOver = false;
        private bool hasMouseLeft = false;

        public OSButton(uint x, uint y, uint width, uint height, Color color)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.color = color;
            usingImage = false;
            imageHasAlpha = false;
        }

        public OSButton(uint x, uint y, Bitmap image, bool hasAlpha)
        {
            this.x = x;
            this.y = y;
            this.image = image;
            this.height = image.Height;
            this.width = image.Width;
            usingImage = true;
            imageHasAlpha = hasAlpha;
        }

        public void ButtonScreenUpdate(Canvas canvas)
        {
            if (usingImage)
            {
                if (imageHasAlpha)
                    canvas.DrawImageAlpha(image, (int)x, (int)y);
                else
                    canvas.DrawImage(image, (int)x, (int)y);
            }
            else
                canvas.DrawFilledRectangle(color, (int)x, (int)y, (int)width, (int)height);
        }

        public void ProcessInput()
        {

            uint mouseX = MouseManager.X;
            uint mouseY = MouseManager.Y;

            if (usingImage)
            {
                if (mouseX >= x && mouseX <= x + image.Width && mouseY >= y && mouseY <= y + image.Height)
                {
                    if(!isMouseOver)
                    {
                        onMouseHover();
                        isMouseOver = true;
                        hasMouseLeft = false;
                    }                    
                    if (MouseManager.MouseState == MouseState.Left)
                        onButtonPressed();
                }
                else if(!hasMouseLeft)
                {
                    isMouseOver = false;
                    hasMouseLeft = true;
                    onMouseHoverLeft();
                }
            }
            else
            {
                if (mouseX >= x && mouseX <= x + width && mouseY >= y && mouseY <= y + height)
                {
                    if (!isMouseOver)
                    {
                        onMouseHover();
                        isMouseOver = true;
                        hasMouseLeft = false;
                    }
                    if (MouseManager.MouseState == MouseState.Left)
                        onButtonPressed();
                }
                else if (!hasMouseLeft)
                {
                    isMouseOver = false;
                    hasMouseLeft = true;
                    onMouseHoverLeft();
                }
            }
        }
        public virtual void onButtonPressed() { }
        public virtual void onMouseHover() { }
        public virtual void onMouseHoverLeft() { }
    }
}
