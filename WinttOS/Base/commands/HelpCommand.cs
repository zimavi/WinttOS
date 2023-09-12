using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils;
using WinttOS.Base.Utils.Commands;
using WinttOS.Base.Utils.Debugging;

namespace WinttOS.Base.commands
{
    public class HelpCommand : Command
    {
        public HelpCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.addCommandUsageStrToManager("Test");
            HelpCommandManager.addCommandUsageStrToManager("Test2");
            HelpCommandManager.addCommandUsageStrToManager("Test3");
            HelpCommandManager.addCommandUsageStrToManager("Test4");
            HelpCommandManager.addCommandUsageStrToManager("Test5");
            HelpCommandManager.addCommandUsageStrToManager("Test6");
            HelpCommandManager.addCommandUsageStrToManager("Test7");
            HelpCommandManager.addCommandUsageStrToManager("Test8");
            HelpCommandManager.addCommandUsageStrToManager("Test9");
            HelpCommandManager.addCommandUsageStrToManager("Test10");
            HelpCommandManager.addCommandUsageStrToManager("Test11");
            HelpCommandManager.addCommandUsageStrToManager("Test12");
        }

        public override string execute(string[] arguments)
        {
            List<string> helpStrs = HelpCommandManager.getCommandsUsageStringsAsList();
            int index = 0;
            for(; index < 24; index++)
            {
                Console.WriteLine(helpStrs[index]);
            }
            Console.Write($"Press Spacebar to continue list ({index + 1}/{helpStrs.Count})...");
            while(true)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);
                if(info.Key == ConsoleKey.Spacebar)
                {
                    if (index >= helpStrs.Count)
                        return "";
                    ShellUtils.MoveCursorUp(-1);
                    ShellUtils.ClearCurrentConsoleLine();
                    Console.WriteLine(helpStrs[index++]);
                    Console.Write($"Press Enter to continue list ({index + 1}/{helpStrs.Count})");
                }
            }
        }
    }
}
