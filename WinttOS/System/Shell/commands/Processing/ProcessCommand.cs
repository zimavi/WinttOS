using System;
using WinttOS.Core.Utils.Debugging;
using WinttOS.System.Processing;
using WinttOS.System.Shell.Utils.Commands;
using WinttOS.System.Users;

namespace WinttOS.System.Shell.Commands.Processing
{
    public class ProcessCommand : Command
    {
        public ProcessCommand(string name) : base(name, User.AccessLevel.Administrator)
        {
            HelpCommandManager.AddCommandUsageStrToManager("process [list|kill|_start|restart] - manages _processes");
        }

        public override string Execute(string[] arguments)
        {
            if (arguments.Length == 0)
                return "Usage: process [list|kill|restart]";

            if (arguments[0] == "list")
            {
                WinttOS.ProcessManager.WriteLineProcessesList();
            }
            else if (arguments[0] == "kill")
            {
                if (arguments.Length < 2)
                    return "Usage: process kill <process id> [-f|--force]";

                if (uint.TryParse(arguments[1], out _))
                {
                    if (WinttOS.ProcessManager.TryGetProcessInstance(out Process process, Convert.ToUInt32(arguments[1])))
                    {
                        if (arguments.Length > 2 && (arguments[2] == "-f" || arguments[2] == "--force"))
                        {
                            WinttDebugger.Trace($"TryStopProcess() => {WinttOS.ProcessManager.TryStopProcess(Convert.ToUInt32(arguments[1]))}");
                            return "Done.";
                        }
                        if (!process.IsProcessCritical)
                            WinttOS.ProcessManager.TryStopProcess(Convert.ToUInt32(arguments[1]));
                        else
                            return "Permission denied";
                        return "Done.";
                    }
                    return "There is no such process!";
                }
                else
                {
                    return "Process id must be number that equals or bigger then 0";
                }
            }
            else if (arguments[0] == "_start")
            {
                if (arguments.Length < 2)
                    return "Usage: process _start <process id>";

                if (uint.TryParse(arguments[1], out _))
                {
                    WinttOS.ProcessManager.TryStartProcess(Convert.ToUInt32(arguments[1]));
                    return "Done.";
                }
                else
                {
                    return "Process id must be number that equals or bigger then 0";
                }
            }
            else if (arguments[0] == "restart")
            {
                if (arguments.Length < 2)
                    return "Usage: process restart <process id>";

                if (uint.TryParse(arguments[1], out _))
                {
                    WinttOS.ProcessManager.TryStopProcess(Convert.ToUInt32(arguments[1]));
                    WinttOS.ProcessManager.TryStartProcess(Convert.ToUInt32(arguments[1]));
                    return "Done.";
                }
                else
                {
                    return "Process id must be number that equals or bigger then 0";
                }
            }
            return "process [list|kill|_start|restart] - manages _processes";
        }
    }
}
