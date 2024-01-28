using System;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands
{
    public class TimeCommand : Command
    {
        public TimeCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager("time - get current time");
        }

        public override string Execute(string[] arguments)
        {
            return DateTime.Now.ToString("h:m:s tt");
        }
    }
}
