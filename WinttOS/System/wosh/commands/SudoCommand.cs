using System;
using WinttOS.Core;
using WinttOS.Core.Utils.System;
using WinttOS.System.Users;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands
{
    internal class SudoCommand : Command
    {
        public SudoCommand(string name) : base(name) 
        {
            HelpCommandManager.AddCommandUsageStrToManager("sudo [command] [command args] - give root permissions");
        }

        public override string Execute(string[] arguments)
        {
            if (arguments.Length == 0)
            {
                Console.Write("Enter password: ");
                string pass = Console.ReadLine();

                while (true)
                {
                    TempUser u = WinttOS.UsersManager.RequestAdminAccount("root", pass);

                    if (u.IsNull())
                        goto WrongPasswordMsg;

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"root$0:\\{GlobalData.CurrentDirectory}: ");
                    string input = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Gray;

                    if (input == "exit")
                        break;
                    else if (input == "whoami")
                        Console.WriteLine("root");

                    Console.WriteLine(WinttOS.CommandManager.ProcessInput(ref u, input));
                }
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.Write("Enter password: ");
                string pass = Console.ReadLine();
                TempUser u = WinttOS.UsersManager.RequestAdminAccount("root", pass);

                if (u.IsNull())
                    goto WrongPasswordMsg;

                return WinttOS.CommandManager.ProcessInput(ref u, string.Join(' ', arguments));
            }
            return null;

            WrongPasswordMsg:
            return "Wrong password!";
        }
    }
}
