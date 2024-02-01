using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.wSystem.Shell;
using WinttOS.wSystem.Shell.Utils.Commands;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Screen
{
    public class ClearScreenCommand : Command
    {
        public ClearScreenCommand(string[] name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"clear - clears _screen");
        }
        public override ReturnInfo Execute()
        {
            Console.Clear();
            return new(this, ReturnCode.OK);
        }
    }
}
