using System.Collections.Generic;
using System.Text;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Shell.commands.Misc
{
    public class LogsCommand : Command
    {
        public LogsCommand(string[] commandValues) : base(commandValues)
        {
            CommandManual = new()
            {
                "NAME",
                "       logs - display system logs",
                "",
                "SYNOPSIS",
                "       logs [OPTIONS]",
                "",
                "DESCRIPTION",
                "       The logs command displays system logs based on the provided options. If no option is supplied, it displays all logs existing since the system booted.",
                "",
                "OPTIONS",
                "       --system, -s",
                "               Display system logs only",
                "",
                "       --comsos, -c",
                "               Display cosmos logs only",
                "",
                "       --bootstrap, -b",
                "               Display bootstrap logs only",
                "",
                "       If no flags are provided, the program will display all logs existing since the system booted.",
                "",
                "EXAMPLES",
                "       To display all logs from boot time: ",
                "               $ logs",
                "",
                "       To display only logs made by system:",
                "               $ logs --system",
                "",
                "       To display only logs made by Cosmos:",
                "               $ logs --cosmos",
                "",
                "       To display only logs made by bootstrap:",
                "               $ logs --bootstrap",
                "",
                "AUTHOR",
                "       Written by zimavi"
            };
        }

        public override ReturnInfo Execute()
        {
            var sb = new StringBuilder();

            foreach(var log in Logger.LogList)
            {
                sb.AppendLine(log.DateTime.ToString() + " - " + Logger.ToString(log.Level) + " - " + log.Log);
            }

            SystemIO.STDOUT.PutLine(sb.ToString());
            
            return new(this, ReturnCode.OK);
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments[0] == "--cosmos" || arguments[0] == "-c")
            {
                ShowLogs(LogLevel.Kernel);
                return new(this, ReturnCode.OK);
            }
            else if (arguments[0] == "--system" || arguments[0] == "-s")
            {
                ShowLogs(LogLevel.OS);
                return new(this, ReturnCode.OK);
            }
            else if (arguments[0] == "--bootstrap" || arguments[0] == "-b")
            {
                ShowLogs(LogLevel.Bootstrap);
                return new(this, ReturnCode.OK);
            }
            return new(this, ReturnCode.ERROR_ARG);
        }

        public void ShowLogs(LogLevel level)
        {
            var sb = new StringBuilder();

            foreach (var log in Logger.LogList)
            {
                if (log.Level == level)
                {
                    sb.AppendLine(log.DateTime.ToString() + " - " + Logger.ToString(log.Level) + " - " + log.Log);
                }
            }

            SystemIO.STDOUT.PutLine(sb.ToString());
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("logs");
            SystemIO.STDOUT.PutLine("logs [--cosmos    | -c]");
            SystemIO.STDOUT.PutLine("logs [--system    | -s]");
            SystemIO.STDOUT.PutLine("logs [--bootstrap | -b]");
        }
    }
}
