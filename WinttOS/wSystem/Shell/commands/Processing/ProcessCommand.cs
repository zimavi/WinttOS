using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Processing;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Processing
{
    public sealed class ProcessCommand : Command
    {
        public ProcessCommand(string[] name) : base(name, AccessLevel.Default)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments[0] == "--list" || arguments[0] == "-l")
            {
                WinttOS.ProcessManager.WriteLineProcessesList();
                return new(this, ReturnCode.OK);
            }
            else if (arguments[0] == "--kill" || arguments[0] == "-k")
            {
                if (arguments.Count < 2)
                {
                    return new(this, ReturnCode.ERROR_ARG, "Expected process id!");
                }    

                if (uint.TryParse(arguments[1], out uint num))
                {
                    if (WinttOS.ProcessManager.TryGetProcessInstance(out Process process, num))
                    {
                        if (process.Type.Value < Process.ProcessType.Program.Value)
                        {
                            if (UsersManager.LoggedLevel.Value < AccessLevel.Administrator.Value)
                                return new ReturnInfo(this, ReturnCode.ERROR, "Access denied");
                        }
                        if (arguments.Count > 2 && (arguments[2] == "-f" || arguments[2] == "--force"))
                        {
                            Logger.DoOSLog("[Info] Doing force stop of process " + process.ProcessName + "(PID " + process.ProcessID + ")");
                            WinttOS.ProcessManager.TryStopProcess(num);
                            SystemIO.STDOUT.PutLine("Done.");
                            return new(this, ReturnCode.OK);
                        }
                        if (process.IsProcessCritical)
                        {
                            SystemIO.STDOUT.PutLine("Unable to kill critical process. User '--force' to force kill.");
                            return new(this, ReturnCode.OK);
                        }
                        WinttOS.ProcessManager.TryStopProcess(num);
                        SystemIO.STDOUT.PutLine("Done.");
                        return new(this, ReturnCode.OK);
                    }
                    return new(this, ReturnCode.ERROR, "There is no such process!");
                }
                else
                {
                    return new(this, ReturnCode.ERROR_ARG, "Process id must be number that equals or bigger then 0");
                }
            }
            else if (arguments[0] == "--restart" || arguments[0] == "-r")
            {
                if (arguments.Count < 2)
                {
                    PrintHelp();
                    return new(this, ReturnCode.ERROR_ARG);
                }

                if (uint.TryParse(arguments[1], out uint num))
                {
                    if(WinttOS.ProcessManager.TryGetProcessInstance(out Process process, num))
                    {
                        if (process.Type.Value < Process.ProcessType.Program.Value)
                        {
                            if (UsersManager.LoggedLevel.Value < AccessLevel.Administrator.Value)
                                return new ReturnInfo(this, ReturnCode.ERROR, "Access denied");
                        }
                    }
                    WinttOS.ProcessManager.TryStopProcess(num);
                    WinttOS.ProcessManager.TryStartProcess(num);
                    SystemIO.STDOUT.PutLine("Done.");
                    return new(this, ReturnCode.OK);
                }
                else
                {
                    return new(this, ReturnCode.ERROR_ARG, "Process id must be number that equals or bigger then 0");
                }
            }
            else if(arguments[0] == "--tree" || arguments[0] == "-t")
            {
                var rootProcesses = WinttOS.ProcessManager.GetRootProcesses();

                foreach(var rootProcess in rootProcesses)
                {
                    WinttOS.ProcessManager.PrintProcessTree(rootProcess);
                }

                return new(this, ReturnCode.OK);
            }
            return new(this, ReturnCode.ERROR_ARG, "Flag expected!");
        }

        public override ReturnInfo Execute()
        {
            PrintHelp();
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage: ");
            SystemIO.STDOUT.PutLine("process [--list] [--tree] [--kill] [--restart] [PID]");
        }
    }
}
