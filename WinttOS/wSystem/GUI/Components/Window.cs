using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace WinttOS.wSystem.GUI.Components
{
    public class Window : Component
    {
        public int ZOrder { get; set; }
        public bool IsFocused { get; set; }
        public string Title { get; set; }
        public Cosmos.System.Graphics.Bitmap Icon { get; set; }
        public List<Component> Components { get; private set; }
        public Color BackgroundColor { get; set; }

        public const int TITLEBAR_HIGHT = 32;
        public const int WINDOW_BUTTONS_WIDTH = 32;

        public Window(string title, int x, int y, int width, int height, int zOrder) : base(x, y, width, height)
        {
            Components = new List<Component>();
            Icon = Files.Icons.p16.DefaultApp;
            ZOrder = zOrder;
            Title = title;
            IsFocused = false;
            BackgroundColor = Color.LightGray;
            AddComponent(new ImageButton(Width - 32, -32, Files.Icons.p32.Cross, () =>
            {
                WinttOS.WindowManager.RemoveWindow(this);
            }));
        }
        
        public void AddComponent(Component component) => Components.Add(component);
        
        public void RemoveComponent(Component component) => Components.Remove(component);

        public override void Render(Canvas canvas, int offsetX, int offsetY)
        {
            if(!IsVisable) return;

            int screenWidth = (int)canvas.Mode.Width;
            int screenHeight = (int)canvas.Mode.Height;

            int renderX = Math.Max(0, X);
            int renderY = Math.Max(0, Y);

            int renderWidth;
            if (X < 0)
                renderWidth = Math.Max(0, Math.Min(Width, Width + X));
            else
                renderWidth = Math.Max(0, Math.Min(Width, (int)canvas.Mode.Width - renderX));

            int renderHeight = Math.Max(0, Math.Min(Height, Height + Y));

            if (renderWidth > 0 && renderHeight > 0)
            {
                canvas.DrawFilledRectangle(BackgroundColor, renderX, renderY, renderWidth, renderHeight);

                int renderTitleBarHeight = Math.Min(TITLEBAR_HIGHT, renderHeight);

                canvas.DrawFilledRectangle(Color.Gray, renderX, renderY, renderWidth, renderTitleBarHeight);

                if (renderX + 10 < screenWidth && renderY + 10 < screenHeight)
                {
                    WindowManager.DrawImageAlpha(canvas, Icon, X + 10, Y + 10);
                }
                if (renderX + 20 + 48 < screenWidth && renderY + 20 < screenHeight)
                {
                    WindowManager.DrawString(canvas, Title, Core.Utils.Sys.Files.Fonts.Font18, Color.White, X + 20 + 16, Y + 10);
                }

                canvas.DrawRectangle(Color.White, renderX - 1, renderY, renderWidth, renderHeight);

                foreach (Component component in Components)
                {
                    int compX = component.X + X;
                    int compY = component.Y + Y;
                    int compRenderX = Math.Max(compX, 0);
                    int compRenderY = Math.Max(compY, 0);
                    int compRenderWidth = Math.Min(component.Width, screenWidth - compRenderX);
                    int compRenderHeight = Math.Min(component.Height, screenHeight - compRenderY);

                    if (compRenderWidth > 0 && compRenderHeight > 0)
                    {
                        component.Render(canvas, X, Y + TITLEBAR_HIGHT);
                    }
                }
            }

            IsDirty = false;
        }
        public bool IsTitleBarClicked(int mouseX, int mouseY) =>
            mouseX >= X && mouseX <= X + Width - WINDOW_BUTTONS_WIDTH && mouseY >= Y && mouseY <= Y + TITLEBAR_HIGHT;

        public bool ContainsPoint(int x, int y) =>
            x >= X && x <= X + Width && y >= Y && y <= Y + Height;
    }
}
