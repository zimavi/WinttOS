using Cosmos.System.Graphics;

namespace WinttOS.wSystem.GUI.Components
{
    public abstract class Component
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsVisable { get; set; }

        public bool IsDirty { get; set; }

        public Component(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            IsVisable = true;
            IsDirty = true;
        }

        public abstract void Render(Canvas canvas, int offsetX, int offsetY);
    }
}
