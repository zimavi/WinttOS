using System;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Users;
using System.Collections.Generic;
using Cosmos.System.Coroutines;

namespace WinttOS.wSystem.Shell.Commands.Users
{
    internal class SudoCommand : Command
    {
        public SudoCommand(string[] name) : base(name)
        { }

        private string _passwrd;

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments.Count == 0)
            {
                Console.Write("Enter password: ");
                _passwrd = Console.ReadLine();

                CoroutinePool.Main.AddCoroutine(new(HandleSudoShellAsync()));
                
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.Write("Enter password: ");
                string pass = Console.ReadLine();
                TempUser u = WinttOS.UsersManager.RequestAdminAccount("root", pass);

                if (u.IsNull())
                    goto WrongPasswordMsg;

                WinttOS.CommandManager.ProcessInput(ref u, string.Join(' ', arguments));
            }
            return new(this, ReturnCode.OK);

        WrongPasswordMsg:
            return new(this, ReturnCode.ERROR, "Wrong password!");
        }

        public IEnumerator<CoroutineControlPoint> HandleSudoShellAsync()
        {
            /*  TODO: Make input as it works with shell
            while (true)
            {
                TempUser u = WinttOS.UsersManager.RequestAdminAccount("root", _passwrd);

                if (u.IsNull())
                    yield break;

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
            */
            yield break;
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("- sudo [command]");
        }
    }
}
