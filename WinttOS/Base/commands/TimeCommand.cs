using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils.Commands;

namespace WinttOS.Base.commands
{
    public class TimeCommand : Command
    {
        public TimeCommand(string name) : base(name, false)
        {
            HelpCommandManager.addCommandUsageStrToManager("time - get current time");
        }

        public override string execute(string[] arguments)
        {
            return DateTime.Now.ToString("h:m:s tt");
        }
    }

    public class DateCommand : Command
    {
        public DateCommand(string name) : base(name, false)
        {
            HelpCommandManager.addCommandUsageStrToManager("date [-f --full] - get current date");
        }

        public override string execute(string[] arguments)
        {
            if (arguments.Length == 1 && (arguments[0] == "-f" || arguments[0] == "--full"))
                return DateTime.UtcNow.ToString("dddd, dd MMMM, yyyy | h:m:s tt");
            else
                return DateTime.UtcNow.ToString("dddd, dd MMMM, yyyy");
        }
    }
}
