using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using Sys = Cosmos.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.System;

namespace WinttOS.Core.Utils.GUI
{
    public class OSMouse
    {
        public readonly Canvas Canvas;
        private Bitmap cursor;

        public OSMouse(Canvas canvas)
        {
            Canvas = canvas;
            cursor = new Bitmap(Files.RawCursorImage);
        }

        public void DrawCursor()
        {
            Sys.MouseManager.ScreenWidth = Canvas.Mode.Width;
            Sys.MouseManager.ScreenHeight = Canvas.Mode.Height;

            int X = (int)Sys.MouseManager.X;
            int Y = (int)Sys.MouseManager.Y;

            Canvas.DrawImageAlpha(cursor, X, Y);
        }

    }
}
