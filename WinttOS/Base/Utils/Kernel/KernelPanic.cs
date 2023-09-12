﻿using Cosmos.System;
using Cosmos.System.Graphics;
using System.Drawing;

namespace WinttOS.Base.Utils.Kernel
{
    internal class KernelPanic
    {
        private static Canvas canvas;
        internal KernelPanic(string message, object sender) 
        {
            panic(message, sender);
        }
        private static void panic(string message, object sender)
        {
            if(FullScreenCanvas.IsInUse)
                FullScreenCanvas.Disable();
            canvas = FullScreenCanvas.GetFullScreenCanvas(new(640, 480, ColorDepth.ColorDepth32)); // 1024, 768
            for (int i = 3; i > 0; i--)
            {
                canvas.Clear(Color.Red);
                printCentered("WinttOS has been unexpectedly stopped!", -5);
                printCentered("If you see this message for the first time,", -2);
                printCentered("try to reboot you computer, or contact devs.", -1);
                printCentered("Details:", 1);
                printCentered(message, 2);
                printCentered($"Coused by '{sender.GetType().Name}'", 3);
                printCentered($"Computer will automatically reboot in {i}s", 5);
                canvas.Display();
                Cosmos.HAL.Global.PIT.Wait(1000);
            }
            Power.Reboot();
        }
        private static void printCentered(string message, int yOffset = 0, int xOffset = 0)
        {
            canvas.DrawString(message, GlobalData.FallbackFont, Color.White,
                (int)((canvas.Mode.Width / 2) - ((message.Length / 2 ) * 8) + (xOffset * 8)), 
                (int)((canvas.Mode.Height / 2) + (yOffset * 18)));
        }
    }
}