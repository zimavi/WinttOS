using Cosmos.System.Graphics;
using System;

namespace WinttOS.wSystem.GUI.Components
{
    public class ImageButton : Button
    {
        private Bitmap _image;
        public Bitmap Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                Width = (int)value.Width;
                Height = (int)value.Height;
            }
        }

        public ImageButton(int x, int y, Bitmap image, Action onClick) : base(x, y, (int)image.Width, (int)image.Height, "", onClick)
        {
            _image = image;
        }

        public override void Render(Canvas canvas, int offsetX, int offsetY)
        {
            if (!IsVisable) return;

            int renderX = Math.Max(0, X + offsetX);

            int renderWidth;

            if (X + offsetX < 0)
                renderWidth = Math.Max(0, Math.Min(Width, Width + X + offsetX));
            else
                renderWidth = Math.Max(0, Math.Min(Width, (int)canvas.Mode.Width - renderX));

            if (renderWidth > 0)
            {
                WindowManager.DrawImage(canvas, _image, X + offsetX, Y + offsetY);
            }

            IsDirty = false;
        }
    }
}
