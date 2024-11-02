using Cosmos.Core;
using Cosmos.System.Graphics;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;

namespace WinttOS.Core.Utils.Kernel
{
    internal sealed class KernelPanic
    {
        
        internal KernelPanic(string message, HALException exception)
        {
            panic(message, exception);
        }

        internal KernelPanic(string message, object sender)
        {
            panic(message, sender);
        }
        private static void panic(string message, object sender)
        {

            if(!wSystem.WinttOS.IsTty)
            {
                FullScreenCanvas.Disable();
                wSystem.WinttOS.Tty = new(1920, 1080, true);
                wSystem.WinttOS.IsTty = true;
            }

            wSystem.WinttOS.KernelPrint = true;

            ShellUtils.PrintTaskResult("Fatal", ShellTaskResult.FAILED, "Kernel panic!");


            ShellUtils.PrintTaskResult("Logs", ShellTaskResult.NONE);

            foreach (var log in Logger.LogList)
            {
                ShellUtils.PrintTaskResult("Logs", ShellTaskResult.NONE, log.DateTime.ToString() + " - [" + Logger.ToString(log.Level) + "] - " + log.Log);
            }

            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "kernel panic:");
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, message);
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "kernel panic end");

            CPU.DisableInterrupts();
            CPU.Halt();

            while (true) { }
        }
        private static void panic(string message, HALException exception)
        {
            if (!wSystem.WinttOS.IsTty)
            {
                FullScreenCanvas.Disable();
                wSystem.WinttOS.Tty = new(1920, 1080, true);
                wSystem.WinttOS.IsTty = true;
            }

            wSystem.WinttOS.KernelPrint = true;

            ShellUtils.PrintTaskResult("Fatal", ShellTaskResult.FAILED, "Kernel panic!");

            ShellUtils.PrintTaskResult("Logs", ShellTaskResult.NONE);

            foreach (var log in Logger.LogList)
            {
                ShellUtils.PrintTaskResult("Logs", ShellTaskResult.NONE, log.DateTime.ToString() + " - [" + Logger.ToString(log.Level) + "] - " + log.Log);
            }

            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "kernel panic: " + message);
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "CPU Exception: x" + exception.CTXInterrupt);
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "Description: " + exception.Description);
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "Wintt version: " + wSystem.WinttOS.WinttVersion);
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "Wintt revision: " + wSystem.WinttOS.WinttRevision);
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "Last knows address: " + (exception.LastKnownAddress == "" ? "uknowown" : ("0x" + exception.LastKnownAddress)));
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "kernal panic end");

            CPU.DisableInterrupts();
            CPU.Halt();

            while (true) { }
        }
    }
}
