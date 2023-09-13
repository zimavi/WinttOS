using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Core.Utils.Commands
{

    public static class HelpCommandManager
    {
        private static List<string> helpList = new List<string>();

        public static void addCommandUsageStrToManager(string usage)
        {
            helpList.Add(usage);
        }

        public static List<string> getCommandsUsageStringsAsList() => helpList;
        public static string getCommandsUsageStringsAsString()
        {
            return String.Join('\n', helpList.ToArray());
        }

    }
}
