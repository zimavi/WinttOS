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
        private static WinttStatus _status;
        static double completePercentage = 0;
        private KernelPanic(WinttStatus message, object sender, Exception exception)
        {
            panic(message, sender, exception);
        }

        internal KernelPanic(WinttStatus message, object sender)
        {
            panic(message, sender);
        }
        private static void panic(WinttStatus message, object sender, Exception exception)
        {
            panic(message, sender);
        }
        private static void panic(WinttStatus message, object sender)
        {
            canvas = FullScreenCanvas.GetFullScreenCanvas(new(1024, 768, ColorDepth.ColorDepth32)); // 1024, 768 | 640, 480

            _status = message;

            updateText();
            
            List<string> textToWrite = new()
            {
                "CALL_STACK:",
                WinttCallStack.GetCallStack(),
                "LAST_MESSAGES:"
            };

            completePercentage = 0.5;
            updateText();

            textToWrite.AddRange(WinttDebugger.ErrorMessages);

            completePercentage = 0.9;
            updateText();

            File.WriteAllText(@"0:\core_dump.log", string.Join('\n', textToWrite.ToArray()));

            completePercentage = 1;
            updateText();

            Cosmos.HAL.Global.PIT.Wait(1000);

            Power.Reboot();
        }

        private static void updateText()
        {
            canvas.Clear(Color.Red);
            canvas.DrawString(":(", Files.Fonts.Font18, Color.White, 10, 10);
            canvas.DrawString("WinttOS ran into problem and needs to restart.", Files.Fonts.Font18, Color.White, 10, 30);
            canvas.DrawString("We're just collecting some error info, and then", Files.Fonts.Font18, Color.White, 10, 55);
            canvas.DrawString("we'll restart for you.", Files.Fonts.Font18, Color.White, 10, 80);
            canvas.DrawString($"{completePercentage:P0} complete", Files.Fonts.Font18, Color.White, 10, 110);
            canvas.DrawString($"Stop code: {_status.Name} (0x{_status.Value:X8})", Files.Fonts.Font18, Color.White, 10, 150);
            canvas.Display();
        }
    }
}
