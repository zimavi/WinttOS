using Cosmos.System;
using Cosmos.System.Coroutines;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using Cosmos.System.Network.IPv4.TCP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.System;

namespace WinttOS.Core.Utils.Kernel
{
    internal class KernelPanic
    {
        static Canvas canvas;
        private static WinttStatus status;
        static double completePercentage = 0;
        private KernelPanic(WinttStatus message, object sender, Exception exception)
        {
            panic(message, sender, exception);
        }

        internal KernelPanic(WinttStatus message, object sender)
        {
            Panic(message, sender);
        }
        private static void panic(WinttStatus message, object sender, Exception exception)
        {
            /*
            if (FullScreenCanvas.IsInUse)
                FullScreenCanvas.Disable();
            canvas = FullScreenCanvas.GetFullScreenCanvas(new(640, 480, ColorDepth.ColorDepth32)); // 1024, 768

            canvas.Clear(Color.Red);
            printCentered("WinttOS has been unexpectedly stopped!", -6);
            printCentered("If you see this message for the first time,", -3);
            printCentered("try to reboot you computer, or contact devs.", -2);
            printCentered("Details:", 0);
            printCentered(message, 1);
            printCentered(exception.Message, 2);
            printCentered($"Collecting core dump...", 3);
            canvas.Display();

            List<string> textToWrite = new()
            {
                "CALL_STACK:",
                WinttCallStack.GetCallStack(),
                "LAST_MESSAGES:"
            };

            textToWrite.AddRange(WinttDebugger.ErrorMessages);

            canvas.Clear(Color.Red);
            printCentered("WinttOS has been unexpectedly stopped!", -6);
            printCentered("If you see this message for the first time,", -3);
            printCentered("try to reboot you computer, or contact devs.", -2);
            printCentered("Details:", 0);
            printCentered(message, 1);
            printCentered(exception.Message, 2);
            printCentered($"Saving core dump...", 3);
            canvas.Display();

            if (!File.Exists(@"0:\core_dump.log"))
                File.Create(@"0:\core_dump.log");

            File.WriteAllText(@"0:\core_dump.log", string.Join('\n', textToWrite.ToArray()));

            for (int i = 5; i > 0; i--)
            {
                canvas.Clear(Color.Red);
                printCentered(":(", -6);
                printCentered("If you see this message for the first time,", -3);
                printCentered("try to reboot you computer, or contact devs.", -2);
                printCentered("Details:", 0);
                printCentered(message, 1);
                printCentered(exception.Message, 2);
                printCentered($"In '{sender.GetType().Name}'", 3);
                printCentered($"Computer will automatically reboot in {i}s", 4);
                canvas.Display();
                Cosmos.HAL.Global.PIT.Wait(1000);
            }
            Power.Reboot();
            */
        }
        private static void Panic(WinttStatus message, object sender)
        {
            canvas = FullScreenCanvas.GetFullScreenCanvas(new(1024, 768, ColorDepth.ColorDepth32)); // 1024, 768 | 640, 480

            status = message;

            UpdateText();
            
            List<string> textToWrite = new()
            {
                "CALL_STACK:",
                WinttCallStack.GetCallStack(),
                "LAST_MESSAGES:"
            };

            completePercentage = 0.5;
            UpdateText();

            textToWrite.AddRange(WinttDebugger.ErrorMessages);

            completePercentage = 0.9;
            UpdateText();

            File.WriteAllText(@"0:\core_dump.log", string.Join('\n', textToWrite.ToArray()));

            completePercentage = 1;
            UpdateText();

            Cosmos.HAL.Global.PIT.Wait(1000);

            Power.Reboot();
        }

        private static void UpdateText()
        {
            canvas.Clear(Color.Red);
            canvas.DrawString(":(", Files.Fonts.Font18, Color.White, 10, 10);
            canvas.DrawString("WinttOS ran into problem and needs to restart.", Files.Fonts.Font18, Color.White, 10, 30);
            canvas.DrawString("We're just collecting some error info, and then", Files.Fonts.Font18, Color.White, 10, 55);
            canvas.DrawString("we'll restart for you.", Files.Fonts.Font18, Color.White, 10, 80);
            canvas.DrawString($"{completePercentage:P0} complete", Files.Fonts.Font18, Color.White, 10, 110);
            canvas.DrawString($"Stop code: {status.Name} (0x{status.Value:X8})", Files.Fonts.Font18, Color.White, 10, 150);
            canvas.Display();
        }
    }
}
