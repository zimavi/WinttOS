﻿using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.Programs.RunCommands
{
    //
    // CAT VERISON 0.1.0
    //
    public class CatUtilCommand : Command
    {
        public CatUtilCommand(string name) : base(name, false)
        {
            HelpCommandManager.addCommandUsageStrToManager(@"cat <path\to\file> - reads all text from file (use 'man cat' for more info)");
        }

        public override string Execute(string[] arguments)
        {
            CAT instance = new CAT();
            return instance.Execute(arguments);
        }
    }
}
