using System.Collections.Generic;
using WinttOS.Core.Utils.Cryptography;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Users
{
    internal sealed class SudoCommand : Command
    {
        public SudoCommand(string[] name) : base(name)
        { }


        public override ReturnInfo Execute(List<string> arguments)
        {
            SystemIO.STDOUT.Put("Enter password: ");
            string pass = SystemIO.STDIN.Get(true);

            if (!UsersManager.GetUser("user:root").Contains(Sha256.hash(pass)))
                return new(this, ReturnCode.ERROR, "Invalid password. Try again");

            AccessLevel level = UsersManager.LoggedLevel;
            UsersManager.LoggedLevel = AccessLevel.SuperUser;
            WinttOS.CommandManager.ProcessInput(string.Join(' ', arguments.ToArray()));
            UsersManager.LoggedLevel = level;

            return new(this, ReturnCode.OK);            
        }


        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("sudo [command]");
        }
    }
}
