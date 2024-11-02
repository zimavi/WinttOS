using Cosmos.System.Graphics;
using System.Drawing;
using Fonts = Cosmos.System.Graphics.Fonts;

namespace WinttOS.wSystem.GUI.Components
{
    public class Label : Component
    {
        public string Text { get; set; }
        public Color TextColor { get; set; }
        public Fonts.Font Font { get; set; }
        public Label(int x, int y, int width, int height, string text) : base(x, y, width, height)
        {
            Text = text;
            TextColor = Color.Black;
            Font = Core.Utils.Sys.Files.Fonts.Font18;
        }

        public override void Render(Canvas canvas, int offsetX, int offsetY)
        {
            if(!IsVisable) return;

            WindowManager.DrawString(canvas, Text, Font, TextColor, X + offsetX, Y + offsetY);

            IsDirty = false;
        }
    }
}
