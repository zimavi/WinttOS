﻿using WinttOS.Base.Utils.Commands;

namespace WinttOS.Base.commands
{
    public class WhoAmICommand : Command
    {
        public WhoAmICommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.addCommandUsageStrToManager("whoami - get current active account");
        }

        public override string execute(string[] arguments)
        {
            return Kernel.UsersManager.currentUser.Login;
        }
    }
}