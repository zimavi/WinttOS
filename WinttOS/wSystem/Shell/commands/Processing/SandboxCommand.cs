using WinttOS.wSystem.Shell.Programs;
using WinttOS.wSystem.Shell.Utils.Commands;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Processing
{
    internal class SandboxCommand : Command
    {
        public SandboxCommand(string name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager("sandbox (for now, only works with test class)");
        }

        public override string Execute(string[] arguments)
        {
            uint PID;

            if (!WinttOS.ProcessManager.TryRegisterProcess(new Sandbox(arguments[0]), out PID))
            {
                return "Error!";
            }

            if (!WinttOS.ProcessManager.TryStartProcess(PID))
            {
                return "Error!";
            }

            return "";
        }

    }
}
