using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using Sys = Cosmos.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Base.Utils.GUI
{
    public class OSMouse
    {
        private readonly Canvas _canvas;
        private Bitmap _cursor;
        [ManifestResourceStream(ResourceName = "WinttOS.Base.resources.cur.bmp")]
        static byte[] cursorbytes;

        public OSMouse(Canvas canvas)
        {
            _canvas = canvas;
            _cursor = new Bitmap(cursorbytes);
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
