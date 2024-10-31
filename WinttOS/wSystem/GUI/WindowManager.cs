using Cosmos.HAL;
using Cosmos.System;
using Cosmos.System.Graphics;
using System.Drawing;
using WinttOS.wSystem.Processing;
using WinttOS.wSystem.Services;

namespace WinttOS.wSystem.GUI
{
    public class WindowManager : Service
    {
        private Mode screenMode;
        private Canvas screen;
        private Mouse mouse;

        private uint frame;
        private uint lastTick;
        private uint fps;
        public WindowManager(Mode ScreenMode) : base("Winmand", "WindowManager")
        {
            screenMode = ScreenMode;
        }

        public override void Initialize()
        {
            base.Initialize();

            mouse = new(Files.CursorImage);
        }

        public override void OnServiceStart()
        {

            if (WinttOS.IsTty)
            {
                FullScreenCanvas.Disable();
                WinttOS.IsTty = false;
            }

            screen = FullScreenCanvas.GetFullScreenCanvas(screenMode);

            MouseManager.ScreenWidth = screenMode.Width;
            MouseManager.ScreenHeight = screenMode.Height;
            MouseManager.X = screenMode.Width / 2;
            MouseManager.Y = screenMode.Height / 2;

        }

        public override void OnServiceStop()
        {

            screen.Disable();

            screen = null;
        }

        public override void OnServiceTick()
        {
            screen.Clear();

            frame++;
            uint currTick = RTC.Second;
            if(currTick != lastTick)
            {
                fps = frame;
                frame = 0;
                lastTick = currTick;
            }

            screen.DrawImage(Files.Bg0, 0, 0);

            // windows display logic

            if (mouse.IsMouseVisible)
            {
                screen.DrawImageAlpha(mouse.Cursor, (int)mouse.X, (int)mouse.Y);
            }

            screen.DrawString("FPS: " + fps, Core.Utils.Sys.Files.Fonts.Font18, Color.Black, 10, 10);

            screen.Display();
        }
    }
}
