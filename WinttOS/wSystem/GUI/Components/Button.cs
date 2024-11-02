using Cosmos.System.Graphics;
using Fonts = Cosmos.System.Graphics.Fonts;
using System.Drawing;
using System;

namespace WinttOS.wSystem.GUI.Components
{
    public class Button : Component
    {
        public string Text { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
        public Fonts.Font Font { get; set; }
        public Action OnClick { get; set; }
        public bool HasClicked { get; set; }

        public Button(int x, int y, int width, int height, string text, Action onClick) : base(x, y, width, height)
        {
            Text = text;
            BackgroundColor = Color.DarkGray;
            TextColor = Color.Black;
            Font = Core.Utils.Sys.Files.Fonts.Font18;
            OnClick = onClick;
        }

        public override void Render(Canvas canvas, int offsetX, int offsetY)
        {
            if(!IsVisable) return;

            int renderX = Math.Max(0, X + offsetX);
            int renderY = Math.Max(0, Y + offsetY);

            int renderWidth;

            if (X + offsetX < 0)
                renderWidth = Math.Max(0, Math.Min(Width, Width + X + offsetX));
            else
                renderWidth = Math.Max(0, Math.Min(Width, (int)canvas.Mode.Width - renderX));

            int renderHeight = Math.Max(0, Math.Min(Height, Height + Y + offsetY));

            if (renderWidth > 0)
            {
                canvas.DrawFilledRectangle(BackgroundColor, renderX, renderY, renderWidth, renderHeight);
                canvas.DrawRectangle(Color.Black, renderX - 1, renderY, renderWidth, renderHeight);
                WindowManager.DrawString(canvas, Text, Font, TextColor, X + offsetX, Y + offsetY);
            }

            IsDirty = false;
        }

        public bool IsClicked(int mouseX, int mouseY, int offsetX, int offsetY)
        {
            return mouseX >= X + offsetX && mouseX <= X + offsetX + Width && mouseY >= Y + offsetY && mouseY <= Y + offsetY + Height;
        }
    }
}
