using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.System.Shell;
using WinttOS.System.Shell.Utils.Commands;
using WinttOS.System.Users;

namespace WinttOS.System.Shell.Commands.Screen
{
    public class ClearScreenCommand : Command
    {
        public ClearScreenCommand(string name) : base(name, User.AccessLevel.Guest)
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
