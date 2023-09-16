using WinttOS.System.wosh;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands
{
    public class WhoAmICommand : Command
    {
        public WhoAmICommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.addCommandUsageStrToManager("whoami - get current active account");
        }

        public override string execute(string[] arguments)
        {
            return WinttOS.UsersManager.currentUser.Login;
        }
    }
}
