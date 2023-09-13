using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using Sys = Cosmos.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Core.Utils.GUI
{
    public class OSMouse
    {
        public readonly Canvas _canvas;
        private Bitmap _cursor;

        public OSMouse(Canvas canvas)
        {
            _canvas = canvas;
            _cursor = new Bitmap(Files.RawCursorImage);
        }

        public void DrawCursor()
        {
            Sys.MouseManager.ScreenWidth = _canvas.Mode.Width;
            Sys.MouseManager.ScreenHeight = _canvas.Mode.Height;

            int X = (int)Sys.MouseManager.X;
            int Y = (int)Sys.MouseManager.Y;

            _canvas.DrawImageAlpha(_cursor, X, Y);
        }

    }
}
