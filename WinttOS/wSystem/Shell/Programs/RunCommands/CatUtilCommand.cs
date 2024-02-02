using System;
using System.Collections.Generic;

namespace WinttOS.wSystem.Shell.Programs.RunCommands
{
    //
    // CAT VERISON 0.1.0
    //
    public class CatUtilCommand : Command
    {
        public CatUtilCommand(string[] name) : base(name, false)
        {
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            CAT instance = new CAT();
            Console.WriteLine(instance.Execute(arguments.ToArray()));
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("- cat {file}");
        }
    }
}
