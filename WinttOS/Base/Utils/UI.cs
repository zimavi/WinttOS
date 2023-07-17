using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Cosmos.System.Graphics;
using System.Drawing;
using WinttOS.Base.Utils.GUI;

namespace WinttOS.Base.Utils
{
    public class UI
    {
        public Canvas _canvas { get; private set; }
        public OSMouse _mouse { get; private set; }

        public void InitializeUI()
        {
            _canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(1920, 1080, ColorDepth.ColorDepth32));
            _mouse = new OSMouse(_canvas);
        }
        public void InitializeUI(uint modeW, uint modeH)
        {
            _canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(modeW, modeH, ColorDepth.ColorDepth32));
            _mouse = new OSMouse(_canvas);
        }
        public void InitializeUI(uint modeW, uint modeH, ColorDepth depth)
        {
            _canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(modeW, modeH, depth));
            _mouse = new OSMouse(_canvas);
        }
        public void InitializeUI(Mode mode)
        {
            _canvas = FullScreenCanvas.GetFullScreenCanvas(mode);
            _mouse = new OSMouse(_canvas);
        }
    }
}
