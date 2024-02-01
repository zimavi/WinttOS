using System;
using System.Collections.Generic;
using WinttOS.wSystem.Shell.Utils.Commands;

namespace WinttOS.wSystem.Shell.Programs.RunCommands
{
    //
    // CAT VERISON 0.1.0
    //
    public class CatUtilCommand : Command
    {
        public CatUtilCommand(string[] name) : base(name, false)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"cat <path\to\file> - reads all text from file (use 'man cat' for more info)");
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            CAT instance = new CAT();
            Console.WriteLine(instance.Execute(arguments.ToArray()));
            return new(this, ReturnCode.OK);
        }
    }
}
