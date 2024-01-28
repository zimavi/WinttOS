using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.System.wosh.Utils.Commands
{

    public static class HelpCommandManager
    {
        private static List<string> helpList = new List<string>();

        public static void AddCommandUsageStrToManager(string usage)
        {
            helpList.Add(usage);
        }

        public static List<string> GetCommandsUsageStringsAsList() => helpList;
        public static string GetCommandsUsageStringsAsString()
        {
            return String.Join('\n', helpList.ToArray());
        }

    }
}
