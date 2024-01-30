using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.System.wosh;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands.Screen
{
    public class ClearScreenCommand : Command
    {
        public ClearScreenCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"clear - clears _screen");
        }
        public override string Execute(string[] arguments)
        {
            Console.Clear();
            return string.Empty;
        }
    }
}
