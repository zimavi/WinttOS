using WinttOS.System.wosh;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands
{
    public class WhoAmICommand : Command
    {
        public WhoAmICommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager("whoami - get current active account");
        }

        public override string Execute(string[] arguments)
        {
            return WinttOS.UsersManager.CurrentUser.Login;
        }
    }
}
