﻿using System;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands
{
    public class TimeCommand : Command
    {
        public TimeCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.addCommandUsageStrToManager("time - get current time");
        }

        public override string Execute(string[] arguments)
        {
            return DateTime.Now.ToString("h:m:s tt");
        }
    }

    public class DateCommand : Command
    {
        public DateCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.addCommandUsageStrToManager("date [-f --full] - get current date");
        }

        public override string Execute(string[] arguments)
        {
            if (arguments.Length == 1 && (arguments[0] == "-f" || arguments[0] == "--full"))
                return DateTime.UtcNow.ToString("dddd, dd MMMM, yyyy | h:m:s tt");
            else
                return DateTime.UtcNow.ToString("dddd, dd MMMM, yyyy");
        }
    }
}