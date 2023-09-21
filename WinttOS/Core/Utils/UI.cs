using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Cosmos.System.Graphics;
using System.Drawing;
using WinttOS.Core.Utils.GUI;

namespace WinttOS.Core.Utils
{
    public class UI
    {
        public Canvas Canvas { get; private set; }
        public OSMouse Mouse { get; private set; }

        public void InitializeUI()
        {
            Canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(1920, 1080, ColorDepth.ColorDepth32));
            InitializeMouseInstence();
        }
        public void InitializeUI(uint modeW, uint modeH)
        {
            Canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(modeW, modeH, ColorDepth.ColorDepth32));
            InitializeMouseInstence();
        }
        public void InitializeUI(uint modeW, uint modeH, ColorDepth depth)
        {
            Canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(modeW, modeH, depth));
            InitializeMouseInstence();
        }
        public void InitializeUI(Mode mode)
        {
            Canvas = FullScreenCanvas.GetFullScreenCanvas(mode);
            InitializeMouseInstence();
        }

        private void InitializeMouseInstence() => Mouse = new OSMouse(Canvas);
    }
}
