using Cosmos.Core;
using Cosmos.System.Graphics;
using WinttOS.Core;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Users;
using WinttOS.wSystem.wAPI;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public sealed class SystemInfoCommand : Command
    {
        public SystemInfoCommand(string[] name) : base(name, User.AccessLevel.Guest)
        { }

        public override ReturnInfo Execute()
        {
            var _cpuName = "Unknown";
            
            if (CPU.CanReadCPUID() != 0)
            {
                _cpuName = CPU.GetCPUBrandString();
            }

            string info =
                "                          =@                         " + "\n" +
                "                          @@@                        " + "\n" +
                "                  =@@@*. @@@@% =#@@@                 " + "root@localhost"+ "\n" +
                "              +%   -@@@@  @@+ @@@@@    #.            " + "Computer name:               wintt-pc" + "\n" +
                "              @@@   =@@@@*=*.@@@@@    @@@            " + "Operation system name:       WinttOS" + "\n" +
                "             #@@@@%-@#  :@@@@@   @@:=@@@@:           " + "Kernel name:                 Cosmos-devkit" + "\n" +
                "       =@@@@# @@@ -@@@@+  *@  :@@@@% %@@% -+*%@-     " + ".NET version:                6.0" +"\n" +
                "        -@@@@+ @@  @@@@@  :+  @@@@@% =@+ #@@@@+      " + "Operation system version:    " + WinttOS.WinttVersion + "\n" +
                "          **+*@@@=  %@#.@=-+ @+:@@.  @@%@@@@@.       " + "Operation system revision:   " + WinttOS.WinttRevision + "\n" +
                "        .@@@%@@@@@=  +-   @@*   @   #@@@@+:#@:       " + "Date and time:               " + Time.DayString() + "/" + Time.MonthString() + "/" + Time.YearString() + ", " + Time.TimeString(true, true, true) + "\n" +
                "       @@@@@@      +@%@   :+   -##@-    :@@@@@@      " + "System boot time:            " + WinttOS.BootTime + "\n" +
                "      =#@@@*  @@#-:+%@@@- :+  *@@%+::*@@  -@@@@@:    " + "Total memory:                " + Memory.TotalMemory + " MB" + "\n" +
                "           :@@@@@@       +@@@.      @@@@@@@          " + "Used memory:                 " + Memory.GetUsedMemory() + " MB (" + WinttOS.MemoryManager.UsedPercentage + "%)" + "\n" +
                "        .:-==*@@@@@@+. =@+*%-@*  .+@@@@@@#=          " + "Free memory:                 " + Memory.GetFreeMemory() + " MB (" + WinttOS.MemoryManager.FreePercentage + "%)" + "\n" +
                "      -@@@@@-       *@@   -*   %@%        @@@@@@     " + "Processor:                   " + _cpuName + "\n" +
                "        #@@@@@@@@%@= ==   +@   .# :@+-#@@@@@@@-      " + "Resolution:                  " + (FullScreenCanvas.IsInUse ? FullScreenCanvas.GetCurrentFullScreenCanvas().Mode.ToString() : "640x480@8") + "\n" +
                "          .   @@@=   @   @@%@*  @=  -@@@:            " + "\n" +
                "        *@@@@+ @@  #@@@@# -*  @@@@@. @@.+@@@@.       " + "\n" +
                "       @@@@@* *@@  @@@@@  -*  :@@@@+ *@# #@@@@-      " + "\n" +
                "             @@@@+@@@@:  +@@@   @@@@@@@@@.   :=      " + "\n" +
                "             #@@@   @  -@@@@@@@  %   @@@%            " + "\n" +
                "              @@    @@@@# *@  @@@@+   @@.            " + "\n" +
                "                   @@@@@ @@@@ -@@@@+                 " + "\n" +
                "                   .     @@@@      .                 " + "\n" +
                "                          @@                         ";

            SystemIO.STDOUT.PutLine(info);
            return new(this, ReturnCode.OK);
        }
    }
}
