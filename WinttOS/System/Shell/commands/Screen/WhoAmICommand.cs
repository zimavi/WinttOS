using WinttOS.System.Shell.Utils.Commands;
using WinttOS.System.Users;

namespace WinttOS.System.Shell.Commands.Screen
{
    public class WhoAmICommand : Command
    {
        public WhoAmICommand(string name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager("whoami - get current active account");
        }

        public override string Execute(string[] arguments)
        {
            return WinttOS.UsersManager.CurrentUser.Login;
        }
    }
}
