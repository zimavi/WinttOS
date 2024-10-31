using Cosmos.System;
using Cosmos.System.Graphics;

namespace WinttOS.wSystem.GUI
{
    public class Mouse
    {
        public Bitmap Cursor;

        public bool IsMouseVisible = true;

        public Mouse() : this(Files.CursorImage) { }

        public Mouse(Bitmap cursor)
        {
            Cursor = cursor;
        }

        public uint X { get { return MouseManager.X; } }
        public uint Y { get { return MouseManager.Y; } }
    }
}
