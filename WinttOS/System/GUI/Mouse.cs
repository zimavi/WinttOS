using Cosmos.System;
using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.System.GUI
{
    public class Mouse
    {
        public Bitmap cursor;
        
        public void DrawCursor()
        {
            uint x = MouseManager.X;
            uint y = MouseManager.Y;

            WinttOS.SystemCanvas.DrawImageAlpha(cursor, (int)x, (int)y);
        }
    }
}
