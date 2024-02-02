using System;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Screen
{
    public class WhoAmICommand : Command
    {
        public WhoAmICommand(string[] name) : base(name, User.AccessLevel.Guest)
        { }

        public override ReturnInfo Execute()
        {
            Console.WriteLine(WinttOS.UsersManager.CurrentUser.Login);
            return new(this, ReturnCode.OK);
        }
    }
}
