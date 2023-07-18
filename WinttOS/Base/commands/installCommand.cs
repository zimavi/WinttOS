using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinttOS.Base.GUI;
using WinttOS.Base.Utils;
using WinttOS.Base.Utils.GUI;

namespace WinttOS.Base.commands
{
    public class installCommand : Command
    {
        private static Install installUI;
        private static InstallButton button1;

        [ManifestResourceStream(ResourceName = "WinttOS.Base.resources.os_next_inst_img.bmp")]
        private byte[] button1_img;

        public installCommand(string name) : base(name) { }

        public override string execute(string[] arguments)
        {
            try
            {
                GlobalData.ui = new UI();
                GlobalData.ui.InitializeUI(new Mode(1920, 1080, ColorDepth.ColorDepth32));
                installUI = new Install(GlobalData.ui._canvas);
                Bitmap bitmap = new Bitmap(button1_img);
                button1 = new InstallButton(GlobalData.ui._canvas, 1207, 795, bitmap);

                //Thread th = new Thread(ScreenUpdateWorker);
                //th.Start();

                ScreenUpdateWorker();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        private static void ScreenUpdateWorker()
        {
            while(true)
            {
                // Draw button and check if mouse button is pressed
                button1.isMouseOver((int)Cosmos.System.MouseManager.X, (int)Cosmos.System.MouseManager.Y);
                // Draw mouse cursor
                GlobalData.ui._mouse.DrawCursor();
                // Finish frame
                GlobalData.ui._canvas.Display();
                // Start new frame
                GlobalData.ui._canvas.Clear();
                // Show background
                installUI.UpdateWallpaper();
            }
        }

    }
}
