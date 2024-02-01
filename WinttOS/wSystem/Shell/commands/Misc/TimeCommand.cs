using System;
using WinttOS.wSystem.Shell.Utils.Commands;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public class TimeCommand : Command
    {
        public TimeCommand(string[] name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager("time - get current time");
        }

        public override ReturnInfo Execute()
        {
            Console.WriteLine(DateTime.Now.ToString("h:m:s tt"));
            return new(this, ReturnCode.OK);
        }
    }
}
