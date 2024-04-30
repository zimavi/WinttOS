﻿using Cosmos.System;
using Cosmos.System.Graphics;

namespace WinttOS.wSystem.GUI
{
    public class Mouse
    {
        public Bitmap Cursor;

        public bool IsMouseVisible;

        public Mouse() : this(new(Files.RawCursorImage)) { }

        public Mouse(Bitmap cursor)
        {
            Cursor = cursor;
        }
        
        public void DrawCursor()
        {
            if (!IsMouseVisible)
                return;

            uint x = MouseManager.X;
            uint y = MouseManager.Y;

            WinttOS.SystemCanvas.DrawImageAlpha(Cursor, (int)x, (int)y);
        }
    }
}
