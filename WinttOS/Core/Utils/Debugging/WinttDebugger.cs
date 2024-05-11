using System;
using System.Collections.Generic;

namespace WinttOS.Core.Utils.Debugging
{
    public enum LogLevel
    {
        Kernel,
        Bootstrap,
        OS
    }

    public sealed class LogEntry
    {
        public LogLevel Level;
        public string Log;
        public DateTime DateTime;

        public LogEntry(LogLevel level, string log, DateTime dateTime)
        {
            Level = level;
            Log = log;
            DateTime = dateTime;
        }

    }
    public static class Logger
    {
        public static List<LogEntry> LogList = new();

        public static void DoKernelLog(string log)
        {
            LogList.Add(new LogEntry(LogLevel.Kernel, log, DateTime.Now));
        }

        public static void DoBootLog(string log)
        {
            LogList.Add(new LogEntry(LogLevel.Bootstrap, log, DateTime.Now));
        }

        public static void DoOSLog(string log)
        {
            LogList.Add(new LogEntry(LogLevel.OS, log, DateTime.Now));
        }

        public static string ToString(LogLevel level)
        {
            switch(level)
            {
                case LogLevel.Kernel:       return "Cosmos OS";
                case LogLevel.Bootstrap:    return "Bootstrap";
                default:                    return "Wintt  OS";
            }
        }
    }
}
