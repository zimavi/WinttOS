using Cosmos.System;
using Cosmos.System.Coroutines;
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
            wSystem.WinttOS.KernelPrint = true;

            ShellUtils.PrintTaskResult("Fatal", ShellTaskResult.FAILED, "Kernel panic!");

            if(wSystem.WinttOS.ProcessManager != null)
            {
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

            ShellUtils.PrintTaskResult("Logs", ShellTaskResult.NONE);

            foreach (var log in Logger.LogList)
            {
                ShellUtils.PrintTaskResult("Logs", ShellTaskResult.NONE, log.DateTime.ToString() + " - [" + Logger.ToString(log.Level) + "] - " + log.Log);
            }

            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "kernel panic:");
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, message);
            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "kernel panic end");

            System.Console.ReadKey(true);

            Power.Reboot();
        }
        private static void panic(string message, HALException exception)
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

            ShellUtils.PrintTaskResult("Logs", ShellTaskResult.NONE);

            foreach (var log in Logger.LogList)
            {
                ShellUtils.PrintTaskResult("Logs", ShellTaskResult.NONE, log.DateTime.ToString() + " - [" + log.Level + "] - " + log.Log);
            }

            ShellUtils.PrintTaskResult("Panic", ShellTaskResult.NONE, "kernel panic: " + message);
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
