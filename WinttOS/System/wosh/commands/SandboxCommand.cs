using Cosmos.System.Coroutines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.System.wosh.Programs;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands
{
    internal class SandboxCommand : Command
    {
        public SandboxCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager("sandbox (for now, only works with test class)");
        }

        public override string Execute(string[] arguments)
        {
            uint PID;

            if(!WinttOS.ProcessManager.TryRegisterProcess(new Sandbox(arguments[0]), out PID))
            {
                return "Error!";
            }

            if(!WinttOS.ProcessManager.TryStartProcess(PID))
            {
                return "Error!";
            }

            return "";
        }

    }
}
