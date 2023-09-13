using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Users;
using WinttOS.Core.Utils;
using WinttOS.Core.Utils.Commands;

namespace WinttOS.Core.commands
{
    internal class SudoCommand : Command
    {
        public SudoCommand(string name) : base(name) 
        {
            HelpCommandManager.addCommandUsageStrToManager("sudo [command] [command args] - give root permissions");
        }

        public override string execute(string[] arguments)
        {
            if (arguments.Length == 0)
            {
                Console.Write("Enter password: ");
                string pass = Console.ReadLine();
                while (true)
                {
                    TempUser u = Kernel.UsersManager.RequestAdminAccount("root", pass);
                    if (u.IsNull())
                        goto WrongPasswordMsg;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"root$0:\\{GlobalData.currDir}: ");
                    string input = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (input == "exit")
                        break;
                    else if (input == "whoami")
                        Console.WriteLine("root");
                    Console.WriteLine(Kernel.manager.processInput(ref u, input));
                }
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.Write("Enter password: ");
                string pass = Console.ReadLine();
                TempUser u = Kernel.UsersManager.RequestAdminAccount("root", pass);
                if (u.IsNull())
                    goto WrongPasswordMsg;
                return Kernel.manager.processInput(ref u, string.Join(' ', arguments));
            }
            return null;

            WrongPasswordMsg:
            return "Wrong password!";
        }
    }
}
