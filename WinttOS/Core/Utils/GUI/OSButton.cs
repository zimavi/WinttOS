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

        public readonly uint X;
        public readonly uint Y;
        public readonly uint Width;
        public readonly uint Height;
        public readonly Color Color;
        public readonly Bitmap Image;
        private readonly bool IsUsingImage;
        public readonly bool DoImageHasAlpha;
        private bool isMouseOver = false;
        private bool hasMouseLeft = false;

        public OSButton(uint x, uint y, uint width, uint height, Color color)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Color = color;
            IsUsingImage = false;
            DoImageHasAlpha = false;
        }

        public OSButton(uint x, uint y, Bitmap image, bool hasAlpha)
        {
            X = x;
            Y = y;
            Image = image;
            Height = image.Height;
            Width = image.Width;
            IsUsingImage = true;
            DoImageHasAlpha = hasAlpha;
        }

        public void ButtonScreenUpdate(Canvas canvas)
        {
            if (IsUsingImage)
            {
                if (DoImageHasAlpha)
                    canvas.DrawImageAlpha(Image, (int)X, (int)Y);
                else
                    canvas.DrawImage(Image, (int)X, (int)Y);
            }
            else
                canvas.DrawFilledRectangle(Color, (int)X, (int)Y, (int)Width, (int)Height);
        }

        public void ProcessInput()
        {

            uint mouseX = MouseManager.X;
            uint mouseY = MouseManager.Y;

            if (IsUsingImage)
            {
                if (mouseX >= X && mouseX <= X + Image.Width && mouseY >= Y && mouseY <= Y + Image.Height)
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
                if (mouseX >= X && mouseX <= X + Width && mouseY >= Y && mouseY <= Y + Height)
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
