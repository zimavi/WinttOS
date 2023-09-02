using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinttOS.Base.GUI;
using WinttOS.Base.Utils;
using WinttOS.Base.Utils.Commands;
using WinttOS.Base.Utils.GUI;

namespace WinttOS.Base.commands
{
    public class installCommand : Command
    {
        private static Bitmap bg;
        [ManifestResourceStream(ResourceName = "WinttOS.Base.resources.os_install_bg.bmp")]
        private static byte[] bgBytes;

        private static InstallNextButton button1;

        [ManifestResourceStream(ResourceName = "WinttOS.Base.resources.os_next_inst_img.bmp")]
        private static byte[] button1_img;

        public installCommand(string name) : base(name, true) 
        {
            HelpCommandManager.addCommandUsageStrToManager(@"install - show install gui (WIP)");
            manual = new List<string>()
            {
                "This command runs install gui, but",
                "it still in development. So it's only test.",
                "If you run this command, to power pc off, use",
                "'Next' button in menu"
            };
        }

        public override string execute(string[] arguments)
        {
            if (!DevModeCommand.isInDebugMode)
                return null;
            //try
            //{
                GlobalData.ui = new UI();
                GlobalData.ui.InitializeUI(new Mode(1920, 1080, ColorDepth.ColorDepth32));
                //installUI = new Install(GlobalData.ui._canvas);
                Bitmap bitmap = new Bitmap(button1_img);
                bg = new Bitmap(bgBytes);
                button1 = new InstallNextButton(1207, 795, bitmap);

                ScreenUpdateWorker();

            //} catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return String.Empty;
        }

        private static void ScreenUpdateWorker()
        {
            while(true)
            {
                // Show background
                //installUI.UpdateWallpaper();
                GlobalData.ui._canvas.DrawImage(bg, 0, 0);
                // Draw next button
                GlobalData.ui._canvas.DrawImage(button1.image, (int)button1.x, (int)button1.y);
                // Check if mouse button is pressed
                button1.onButtonPressed();
                // Draw mouse cursor
                GlobalData.ui._mouse.DrawCursor();
                // Finish frame
                GlobalData.ui._canvas.Display();
                // Start new frame
                GlobalData.ui._canvas.Clear();
            }
        }

    }
}
