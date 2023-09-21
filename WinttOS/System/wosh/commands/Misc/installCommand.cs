using Cosmos.System.Coroutines;
using Cosmos.System.Graphics;
using System.Collections.Generic;
using WinttOS.Core;
using WinttOS.Core.Utils;
using WinttOS.Core.Utils.GUI;
using WinttOS.Core.Utils.System;
using WinttOS.System.GUI;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands.Misc
{
    public class installCommand : Command
    {
        private static Bitmap bg;

        //private static InstallNextButton button1;
        //private static PowerOffButton button2;
        private static List<OSButton> Buttons = new List<OSButton>();

        public installCommand(string name) : base(name, true, Users.User.AccessLevel.Administrator)
        {
            HelpCommandManager.addCommandUsageStrToManager(@"install - show install gui (WIP)");
            CommandManual = new List<string>()
            {
                "This command runs install gui, but",
                "it still in development. So it's only test.",
                "If you run this command, to power pc off, use",
                "'Next' button in menu"
            };
        }

        public override string Execute(string[] arguments)
        {
            try
            {
                //WinttDebugger.Critical("Executed not implemented command!", true, this);

                if (!DevModeCommand.isInDebugMode)
                    return null;

                GlobalData.UI = new UI();
                GlobalData.UI.InitializeUI(new Mode(1920, 1080, ColorDepth.ColorDepth32));
                Bitmap bitmap = new Bitmap(Files.Installer.RawInstallerNextButtonImage);
                bg = new Bitmap(Files.Installer.RawInstallerBackgroundImage);
                Buttons.Add(new InstallNextButton(1207, 795, bitmap));
                Buttons.Add(new PowerOffButton(1773, 936));


                var coroutine = new Coroutine(ScreenUpdateWorker());
                coroutine.Start();

                CoroutinePool.Main.StartPool();

                return string.Empty;
            }
            catch
            {
                GlobalData.UI.Canvas.Disable();
                GlobalData.UI.Mouse.Canvas.Disable();
                throw;
            }
        }

        private IEnumerator<CoroutineControlPoint> ScreenUpdateWorker()
        {
            while (true)
            {
                // Show background
                GlobalData.UI.Canvas.DrawImage(bg, 0, 0);

                // Buttons tick
                Buttons.ForEach((button) =>
                {
                    button.ButtonScreenUpdate(GlobalData.UI.Canvas);
                    button.ProcessInput();
                });

                // Draw mouse cursor
                GlobalData.UI.Mouse.DrawCursor();

                // Finish frame
                GlobalData.UI.Canvas.Display();

                // Return that Update frame finished
                yield return null;

                // Start new frame
                GlobalData.UI.Canvas.Clear();
            }
        }

    }
}
