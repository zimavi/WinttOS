using System;
using System.Collections.Generic;
using WinttOS.wSystem.Users;
using WinttOS.wSystem.wAPI;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public sealed class TimeCommand : Command
    {
        public TimeCommand(string[] name) : base(name, User.AccessLevel.Guest)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if(arguments.Count > 0)
            {
                string formated = "";
                if (arguments[0] == "-h12")
                {
                    formated = $"{Time.DayString()}.{Time.MonthString()}.{Time.YearString()} {Time.TimeString(true, true, false, true)}";
                }
                else if (arguments[0] == "-h24")
                {
                    formated = $"{Time.DayString()}.{Time.MonthString()}.{Time.YearString()} {Time.TimeString(true, true, false)}";
                }
                else
                {
                    return new(this, ReturnCode.ERROR_ARG, "Expected flag!");
                }

                Console.WriteLine(formated);
                return new(this, ReturnCode.OK);
            }

            return new(this, ReturnCode.ERROR_ARG, "Arguments null!");
        }

        public override ReturnInfo Execute()
        {
            string formated = $"{Time.DayString()}.{Time.MonthString()}.{Time.YearString()} {Time.TimeString(true, true, false)}";
            Console.WriteLine(formated);
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("- time");
            Console.WriteLine("- time -h12");
            Console.WriteLine("- time -h24 (default)");
        }
    }
}
