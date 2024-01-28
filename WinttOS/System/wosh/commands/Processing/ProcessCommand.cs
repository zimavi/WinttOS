using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.System;
using WinttOS.System.Processing;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands.Processing
{
    public class ProcessCommand : Command
    {
        public ProcessCommand(string name) : base(name, Users.User.AccessLevel.Administrator)
        {
            HelpCommandManager.AddCommandUsageStrToManager("process [list|kill|start|restart] - manages processes");
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
                    if (!WinttOS.ProcessManager.GetProcessInstance(Convert.ToUInt32(arguments[1])).IsNull())
                    {
                        if (arguments.Length > 2 && (arguments[2] == "-f" || arguments[2] == "--force"))
                        {
                            WinttDebugger.Trace($"StopProcess() => {WinttOS.ProcessManager.StopProcess(Convert.ToUInt32(arguments[1]))}");
                            return "Done.";
                        }
                        if (!WinttOS.ProcessManager.GetProcessInstance(Convert.ToUInt32(arguments[1])).IsProcessCritical)
                            WinttOS.ProcessManager.StopProcess(Convert.ToUInt32(arguments[1]));
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
            else if (arguments[0] == "start")
            {
                if (arguments.Length < 2)
                    return "Usage: process start <process id>";

                if (uint.TryParse(arguments[1], out _))
                {
                    WinttOS.ProcessManager.StartProcess(Convert.ToUInt32(arguments[1]));
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
                    WinttOS.ProcessManager.StopProcess(Convert.ToUInt32(arguments[1]));
                    WinttOS.ProcessManager.StartProcess(Convert.ToUInt32(arguments[1]));
                    return "Done.";
                }
                else
                {
                    return "Process id must be number that equals or bigger then 0";
                }
            }
            return "process [list|kill|start|restart] - manages processes";
        }
    }
}
