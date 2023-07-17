using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinttOS.Base.Utils;
using WinttOS.Base.Utils.GUI;

namespace WinttOS.Base.commands
{
    public class installCommand : Command
    {
        private static Install installUI;

        public installCommand(string name) : base(name) { }

        public override string execute(string[] arguments)
        {
            GlobalData.ui = new UI();
            GlobalData.ui.InitializeUI(new Mode(1920, 1080, ColorDepth.ColorDepth24));
            installUI = new Install(GlobalData.ui._canvas);

            Thread th = new Thread(ScreenUpdateWorker);
            th.Start();

            for (;;);
        }

        private static void ScreenUpdateWorker()
        {
            while(true)
            {
                GlobalData.ui._mouse.DrawCursor();
                GlobalData.ui._canvas.Display();
            }
        }
    }
}
