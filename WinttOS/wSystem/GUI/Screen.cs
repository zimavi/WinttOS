using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.wSystem.GUI
{
    public class Screen
    {
        private static List<Canvas> _screens = new List<Canvas>();
        private static byte _currentScreenIdx = 0;
        public static Canvas CurrentScreen => _screens[_currentScreenIdx];

        private Canvas _screen;
        private Mouse _mouse;

        public Canvas SystemScreen => _screen;
        public Mouse SystemMouse => _mouse;

        public Screen() : this(new()) { }

        public Screen(Mouse mouse) 
        {
            _screen = FullScreenCanvas.GetFullScreenCanvas();
            this._mouse = mouse;
            _screens.Add(_screen);
        }

        public void Update()
        {
            _mouse.DrawCursor();
        }
    }
}
