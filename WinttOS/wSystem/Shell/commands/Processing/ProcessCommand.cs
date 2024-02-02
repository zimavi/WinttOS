using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.Processing;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Processing
{
    public class ProcessCommand : Command
    {
        public ProcessCommand(string[] name) : base(name, User.AccessLevel.Administrator)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments[0] == "--list")
            {
                WinttOS.ProcessManager.WriteLineProcessesList();
                return new(this, ReturnCode.OK);
            }
            else if (arguments[0] == "--kill")
            {
                if (arguments.Count < 2)
                {
                    PrintHelp();
                    return new(this, ReturnCode.ERROR_ARG);
                }    

                if (uint.TryParse(arguments[1], out uint num))
                {
                    if (WinttOS.ProcessManager.TryGetProcessInstance(out Process process, num))
                    {
                        if (arguments.Count > 2 && (arguments[2] == "-f" || arguments[2] == "--force"))
                        {
                            WinttDebugger.Trace($"TryStopProcess() => {WinttOS.ProcessManager.TryStopProcess(num)}");
                            Console.WriteLine("Done.");
                            return new(this, ReturnCode.OK);
                        }
                        if (!process.IsProcessCritical)
                            WinttOS.ProcessManager.TryStopProcess(num);
                        else
                        {
                            Console.WriteLine("Permission denied");
                            return new(this, ReturnCode.ERROR);
                        }
                        Console.WriteLine("Done.");
                        return new(this, ReturnCode.OK);
                    }
                    Console.WriteLine("There is no such process!");
                    return new(this, ReturnCode.ERROR);
                }
                else
                {
                    Console.WriteLine("Process id must be number that equals or bigger then 0");
                    return new(this, ReturnCode.ERROR_ARG);
                }
            }
            else if (arguments[0] == "--restart")
            {
                if (arguments.Count < 2)
                {
                    PrintHelp();
                    return new(this, ReturnCode.ERROR_ARG);
                }

                if (uint.TryParse(arguments[1], out uint num))
                {
                    WinttOS.ProcessManager.TryStopProcess(num);
                    WinttOS.ProcessManager.TryStartProcess(num);
                    Console.WriteLine("Done.");
                    return new(this, ReturnCode.OK);
                }
                else
                {
                    Console.WriteLine("Process id must be number that equals or bigger then 0");
                    return new(this, ReturnCode.ERROR_ARG);
                }
            }
            PrintHelp();
            return new(this, ReturnCode.ERROR_ARG);
        }

        public override ReturnInfo Execute()
        {
            PrintHelp();
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("- process {--list|--kill|--restart} {PID}");
        }
    }
}
