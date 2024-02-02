using System;
using System.Collections.Generic;

namespace WinttOS.wSystem.Shell.Programs.RunCommands
{
    public class MivCommand : Command
    {
        public MivCommand(string[] name) : base(name, false)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            MIV.StartMIV(arguments[0]);
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("- miv {file}");
        }
    }
}
