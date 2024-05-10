using Cosmos.System;
using Cosmos.System.Coroutines;
using Cosmos.System.Graphics;
using WinttOS.Core.Utils.Sys;

namespace WinttOS.Core.Utils.Kernel
{
    internal sealed class KernelPanic
    {
        private static WinttStatus _status;
        
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
            wSystem.WinttOS.KernelPrint = true;

            ShellUtils.PrintTaskResult("Fatal", ShellTaskResult.FAILED, "Kernel panic!");

            foreach (var p in wSystem.WinttOS.ProcessManager.Processes)
            {
                ShellUtils.PrintTaskResult("Stopping", ShellTaskResult.DOING, p.ProcessName);
                ShellUtils.MoveCursorUp();

                try
                {
                    p.Stop();

                    ShellUtils.PrintTaskResult("Stopping", ShellTaskResult.OK, p.ProcessName);
                }
                catch
                {
                    ShellUtils.PrintTaskResult("Stopping", ShellTaskResult.FAILED, p.ProcessName);
                }
            }

            int i = 0;
            foreach (var c in CoroutinePool.Main.RunningCoroutines)
            {
                ShellUtils.PrintTaskResult("Aborting", ShellTaskResult.DOING, "Thread" + i);
                ShellUtils.MoveCursorUp();

                try
                {
                    c.Stop();
                    ShellUtils.PrintTaskResult("Aborting", ShellTaskResult.OK, "Thread" + i);

                }
                catch
                {
                    ShellUtils.PrintTaskResult("Aborting", ShellTaskResult.FAILED, "Thread" + i);
                }

                i++;
            }

            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "kernel panic:");
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, $"Code: {message.Name}; 0x{message.Value:X8}");
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "kernel panic end");

            System.Console.ReadKey(true);

            Power.Reboot();
        }
        private static void panic(WinttStatus message, HALException exception)
        {

            wSystem.WinttOS.KernelPrint = true;

            ShellUtils.PrintTaskResult("Fatal", ShellTaskResult.FAILED, "Kernel panic!");

            foreach (var p in wSystem.WinttOS.ProcessManager.Processes)
            {
                ShellUtils.PrintTaskResult("Stopping", ShellTaskResult.DOING, p.ProcessName);
                ShellUtils.MoveCursorUp();

                try
                {
                    p.Stop();

                    ShellUtils.PrintTaskResult("Stopping", ShellTaskResult.OK, p.ProcessName);
                }
                catch
                {
                    ShellUtils.PrintTaskResult("Stopping", ShellTaskResult.FAILED, p.ProcessName);
                }
            }

            int i = 0;
            foreach (var c in CoroutinePool.Main.RunningCoroutines)
            {
                ShellUtils.PrintTaskResult("Aborting", ShellTaskResult.DOING, "Thread" + i);
                ShellUtils.MoveCursorUp();

                try
                {
                    c.Stop();
                    ShellUtils.PrintTaskResult("Aborting", ShellTaskResult.OK, "Thread" + i);

                }
                catch
                {
                    ShellUtils.PrintTaskResult("Aborting", ShellTaskResult.FAILED, "Thread" + i);
                }

                i++;
            }

            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "kernel panic: ");
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "CPU Exception: x" + exception.CTXInterrupt);
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "Description: " + exception.Description);
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "Wintt version: " + wSystem.WinttOS.WinttVersion);
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "Wintt revision: " + wSystem.WinttOS.WinttRevision);
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "Last knows address: " + (exception.LastKnownAddress == "" ? "knowown" : ("0x" + exception.LastKnownAddress)));
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "kernal panic end");

            System.Console.ReadKey(true);

            Power.Reboot();
        }
    }
}
