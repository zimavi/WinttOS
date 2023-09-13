
using WinttOS.Core.Utils.Commands;

namespace WinttOS.Core.commands
{
    public class SystemCtlCommand : Command
    {
        public SystemCtlCommand(string name) : base(name, Users.User.AccessLevel.Administrator) 
        {
            HelpCommandManager.addCommandUsageStrToManager("systemctl [--help|-h] - get usage str (or use 'man systemctl')");
            manual = new()
            {

            };
        }

        public override string execute(string[] arguments)
        {
            

            return null;
        }
    }
}
