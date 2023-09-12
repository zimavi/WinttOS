using Cosmos.System.Coroutines;
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
using WinttOS.Base.Utils.Debugging;
using WinttOS.Base.Utils.GUI;

namespace WinttOS.Base.commands
{
    public class installCommand : Command
    {
        private static Bitmap bg;

        //private static InstallNextButton button1;
        //private static PowerOffButton button2;
        private static List<OSButton> Buttons = new List<OSButton>();

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
            try
            {
                //WinttDebugger.Critical("Executed not implemented command!", true, this);

                if (!DevModeCommand.isInDebugMode)
                    return null;

                GlobalData.ui = new UI();
                GlobalData.ui.InitializeUI(new Mode(1920, 1080, ColorDepth.ColorDepth32));
                Bitmap bitmap = new Bitmap(Files.Installer.RawInstallerNextButtonImage);
                bg = new Bitmap(Files.Installer.RawInstallerBackgroundImage);
                Buttons.Add(new InstallNextButton(1207, 795, bitmap));
                Buttons.Add(new PowerOffButton(1773, 936));


                var coroutine = new Coroutine(ScreenUpdateWorker());
                coroutine.Start();

                CoroutinePool.Main.StartPool();

                return String.Empty;
            }
            catch
            {
                GlobalData.ui._canvas.Disable();
                GlobalData.ui._mouse._canvas.Disable();
                throw;
            }
        }

        private IEnumerator<CoroutineControlPoint> ScreenUpdateWorker()
        {
            while (true)
            {
                // Show background
                GlobalData.ui._canvas.DrawImage(bg, 0, 0);

                // Buttons tick
                Buttons.ForEach((button) =>
                {
                    button.ButtonScreenUpdate(GlobalData.ui._canvas);
                    button.ProcessInput();
                });

                // Draw mouse cursor
                GlobalData.ui._mouse.DrawCursor();

                // Finish frame
                GlobalData.ui._canvas.Display();

                // Return that Update frame finished
                yield return null;

                // Start new frame
                GlobalData.ui._canvas.Clear();
            }
        }

    }
}
