using Cosmos.System;
using Cosmos.System.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;

namespace WinttOS.Core.Utils.Kernel
{
    internal sealed class KernelPanic
    {
        private static Canvas _canvas;
        private static WinttStatus _status;
        private static double _completePercentage = 0;
        
        internal KernelPanic(WinttStatus message, HALException exception)
        {
            panic(message, exception);
        }

        internal KernelPanic(WinttStatus message, object sender)
        {
            panic(message, sender);
        }
        private static void panic(WinttStatus message, object sender)
        {
            if (FullScreenCanvas.IsInUse)
                FullScreenCanvas.Disable();
            _canvas = FullScreenCanvas.GetFullScreenCanvas(new(1024, 768, ColorDepth.ColorDepth32)); // 1024, 768 | 640, 480

            _status = message;

            updateText();
            
            List<string> textToWrite = new()
            {
                "CALL_STACK:",
                WinttCallStack.GetCallStack(),
                "LAST_MESSAGES:"
            };

            _completePercentage = 0.5;
            updateText();

            textToWrite.AddRange(WinttDebugger.ErrorMessages);

            _completePercentage = 0.9;
            updateText();

            File.WriteAllText(@"0:\core_dump.log", string.Join('\n', textToWrite.ToArray()));

            _completePercentage = 1;
            updateText();

            Cosmos.HAL.Global.PIT.Wait(1000);

            Power.Reboot();
        }

        private static void updateText()
        {
            _canvas.Clear(Color.Red);
            _canvas.DrawString(":(", Files.Fonts.Font18, Color.White, 10, 10);
            _canvas.DrawString("WinttOS ran into problem and needs to restart.", Files.Fonts.Font18, Color.White, 10, 30);
            _canvas.DrawString("We're just collecting some error info, and then", Files.Fonts.Font18, Color.White, 10, 55);
            _canvas.DrawString("we'll restart for you.", Files.Fonts.Font18, Color.White, 10, 80);
            _canvas.DrawString($"{_completePercentage:P0} complete", Files.Fonts.Font18, Color.White, 10, 110);
            _canvas.DrawString($"Stop code: {_status.Name} (0x{_status.Value:X8})", Files.Fonts.Font18, Color.White, 10, 150);
            _canvas.Display();
        }
        private static void panic(WinttStatus message, HALException exception)
        {
            if (FullScreenCanvas.IsInUse)
                FullScreenCanvas.Disable();
            _canvas = FullScreenCanvas.GetFullScreenCanvas(new(1024, 768, ColorDepth.ColorDepth32)); // 1024, 768 | 640, 480

            _canvas.Clear(Color.Black);
            _canvas.DrawString($"CPU Exception x{exception.CTXInterrupt} occurred in WinttOS", Files.Fonts.Font18, Color.White, 10, 10);
            _canvas.DrawString($"Exception: {exception.Exception}", Files.Fonts.Font18, Color.White, 10, 30);
            _canvas.DrawString($"Description: {exception.Description}", Files.Fonts.Font18, Color.White, 10, 55);
            _canvas.DrawString($"Wintt version: {wSystem.WinttOS.WinttVersion}", Files.Fonts.Font18, Color.White, 10, 80);
            _canvas.DrawString($"Wintt revision: ", Files.Fonts.Font18, Color.White, 10, 110);
            if(exception.LastKnownAddress != "")
            {
                _canvas.DrawString($"Last known address: 0x{exception.LastKnownAddress}", Files.Fonts.Font18, Color.White, 10, 125);
            }
            _canvas.DrawString($"Press any key to reboot...", Files.Fonts.Font18, Color.White, 10, 175);
            _canvas.Display();

            System.Console.ReadKey(true);

            Power.Reboot();
        }
    }
}
