using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.System.GUI
{
    public class Screen
    {
        private static List<Canvas> screens = new List<Canvas>();
        private static byte currentScreenIdx = 0;
        public static Canvas CurrentScreen => screens[currentScreenIdx];

        private Canvas screen;
        private Mouse mouse;

        public Canvas SystemScreen => screen;
        public Mouse SystemMouse => mouse;

        public Screen() : this(new()) { }

        public Screen(Mouse mouse) 
        {
            screen = FullScreenCanvas.GetFullScreenCanvas();
            this.mouse = mouse;
            screens.Add(screen);
        }

        public void Update()
        {
            mouse.DrawCursor();
        }
    }
}
