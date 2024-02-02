using System;
using System.Collections.Generic;
using WinttOS.Core;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.FileSystem
{
    public class MakeDirCommand : Command
    {
        public MakeDirCommand(string[] name) : base(name, User.AccessLevel.Guest)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            GlobalData.FileSystem.CreateDirectory(GlobalData.CurrentDirectory + arguments[0]);
            
            Console.WriteLine("Created directory!");
            return new(this, ReturnCode.OK);
        }
        public override ReturnInfo Execute()
        {
            Console.Write("Enter new dir name: ");
            string dir = Console.ReadLine();
            // Added replacment of spaces in names into _ for preventing unopenable folders
            GlobalData.FileSystem.CreateDirectory(GlobalData.CurrentDirectory + string.Join('_', dir.Split(' ')));
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("- mkdir {directory}");
        }
    }
}
