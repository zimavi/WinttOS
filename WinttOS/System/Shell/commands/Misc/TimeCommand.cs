using System;
using WinttOS.System.Shell.Utils.Commands;
using WinttOS.System.Users;

namespace WinttOS.System.Shell.Commands.Misc
{
    public class TimeCommand : Command
    {
        public TimeCommand(string name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager("time - get current time");
        }

        public override string Execute(string[] arguments)
        {
            return DateTime.Now.ToString("h:m:s tt");
        }
    }
}
