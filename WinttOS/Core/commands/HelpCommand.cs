using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils;
using WinttOS.Core.Utils.Commands;
using WinttOS.Core.Utils.Debugging;

namespace WinttOS.Core.commands
{
    public class HelpCommand : Command
    {
        public HelpCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {

        }

        public override string execute(string[] arguments)
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
