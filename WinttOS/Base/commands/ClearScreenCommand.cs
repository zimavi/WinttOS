using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils.Commands;

namespace WinttOS.Base.commands
{
    public class ClearScreenCommand : Command
    {
        public ClearScreenCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.addCommandUsageStrToManager(@"clear - clears screen");
        }
        public override string execute(string[] arguments)
        {
            Console.Clear();
            return string.Empty;
        }
    }
}
