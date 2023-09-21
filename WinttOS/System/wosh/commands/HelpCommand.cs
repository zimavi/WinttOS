using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.System;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands
{
    public class HelpCommand : Command
    {
        public HelpCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {

        }

        public override string Execute(string[] arguments)
        {
            List<string> helpStrs = HelpCommandManager.getCommandsUsageStringsAsList();
            int index = 0;
            for(; index < 24 && index < helpStrs.Count; index++)
            {
                Console.WriteLine(helpStrs[index]);
            }
            if (index >= helpStrs.Count)
                return "";
            Console.Write($"Press Spacebar to continue list ({index + 1}/{helpStrs.Count})...");
            while(true)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);
                if(info.Key == ConsoleKey.Spacebar)
                {
                    if (index >= helpStrs.Count)
                        return "";
                    ShellUtils.ClearCurrentConsoleLine();
                    ShellUtils.MoveCursorUp(1);
                    WinttDebugger.Debug($"Index: {index}; List count: {helpStrs.Count})", this);
                    Console.WriteLine(helpStrs[index++]);
                    if (index < helpStrs.Count)
                        Console.Write($"Press Enter to continue list ({index + 1}/{helpStrs.Count})");
                }
            }
        }
    }
}
