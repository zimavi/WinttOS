using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.Processing;
using WinttOS.wSystem.Shell.Utils.Commands;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Processing
{
    public class ProcessCommand : Command
    {
        public ProcessCommand(string[] name) : base(name, User.AccessLevel.Administrator)
        {
            HelpCommandManager.AddCommandUsageStrToManager("process [list|kill|_start|restart] - manages _processes");
        }

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

                if (uint.TryParse(arguments[1], out _))
                {
                    if (WinttOS.ProcessManager.TryGetProcessInstance(out Process process, Convert.ToUInt32(arguments[1])))
                    {
                        if (arguments.Count > 2 && (arguments[2] == "-f" || arguments[2] == "--force"))
                        {
                            WinttDebugger.Trace($"TryStopProcess() => {WinttOS.ProcessManager.TryStopProcess(Convert.ToUInt32(arguments[1]))}");
                            Console.WriteLine("Done.");
                            return new(this, ReturnCode.OK);
                        }
                        if (!process.IsProcessCritical)
                            WinttOS.ProcessManager.TryStopProcess(Convert.ToUInt32(arguments[1]));
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

                if (uint.TryParse(arguments[1], out _))
                {
                    WinttOS.ProcessManager.TryStopProcess(Convert.ToUInt32(arguments[1]));
                    WinttOS.ProcessManager.TryStartProcess(Convert.ToUInt32(arguments[1]));
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
