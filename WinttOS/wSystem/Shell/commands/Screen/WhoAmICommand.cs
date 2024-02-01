using System;
using WinttOS.wSystem.Shell.Utils.Commands;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Screen
{
    public class WhoAmICommand : Command
    {
        public WhoAmICommand(string[] name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager("whoami - get current active account");
        }

        public override ReturnInfo Execute()
        {
            Console.WriteLine(WinttOS.UsersManager.CurrentUser.Login);
            return new(this, ReturnCode.OK);
        }
    }
}
