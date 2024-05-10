using System.Collections.Generic;
using System.Text;
using WinttOS.Core.Utils.Sys;

namespace WinttOS.wSystem.Shell.Utils
{
    public static class Misc
    {
        public static List<string> ParseCommandLine(string cmdLine)
        {
            List<string> args = new();

            if (cmdLine.IsNullOrWhiteSpace()) 
                return args;

            StringBuilder currentArg = new();
            bool isInQuoted = false;

            for(int i = 0; i < cmdLine.Length; i++)
            {
                if (cmdLine[i] == '"' || cmdLine[i] == '\'') 
                {
                    if (isInQuoted)
                    {
                        args.Add(currentArg.ToString());
                        currentArg = new();
                        isInQuoted = false;
                    }
                    else
                    {
                        isInQuoted = true;
                    }
                }
                else if (cmdLine[i] == ' ')
                {
                    if (isInQuoted)
                    {
                        currentArg.Append(cmdLine[i]);
                    }
                    else if (currentArg.Length > 0)
                    {
                        args.Add(currentArg.ToString());
                        currentArg = new();
                    }
                }
                else
                {
                    currentArg.Append(cmdLine[i]);
                }
            }

            if (currentArg.Length > 0)
                args.Add(currentArg.ToString());

            return args;
        }
    }
}
