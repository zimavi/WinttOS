using Cosmos.Core;
using Cosmos.System.Graphics;
using System;
using WinttOS.Core;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Users;
using WinttOS.wSystem.wAPI;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public sealed class SystemInfoCommand : Command
    {
        public SystemInfoCommand(string[] name) : base(name, AccessLevel.Default)
        { }

        public override ReturnInfo Execute()
        {
            try
            {

                string info =
                    "                          =@                         " + "\n" +
                    "                          @@@                        " + "\n" +
                    "                  =@@@*. @@@@% =#@@@                 " + "root@localhost" + "\n" +
                    "              +%   -@@@@  @@+ @@@@@    #.            " + "Computer name:               wintt-pc" + "\n" +
                    "              @@@   =@@@@*=*.@@@@@    @@@            " + "Operation system name:       WinttOS" + "\n" +
                    "             #@@@@%-@#  :@@@@@   @@:=@@@@:           " + "Kernel name:                 Cosmos-devkit" + "\n" +
                    "       =@@@@# @@@ -@@@@+  *@  :@@@@% %@@% -+*%@-     " + ".NET version:                6.0" + "\n" +
                    "        -@@@@+ @@  @@@@@  :+  @@@@@% =@+ #@@@@+      " + "Operation system version:    " + WinttOS.WinttVersion + "\n" +
                    "          **+*@@@=  %@#.@=-+ @+:@@.  @@%@@@@@.       " + "Operation system revision:   " + WinttOS.WinttRevision + "\n" +
                    "        .@@@%@@@@@=  +-   @@*   @   #@@@@+:#@:       " + "Date and time:               " + Time.DayString() + "/" + Time.MonthString() + "/" + Time.YearString() + ", " + Time.TimeString(true, true, true) + "\n" +
                    "       @@@@@@      +@%@   :+   -##@-    :@@@@@@      " + "System boot time:            " + WinttOS.BootTime + "\n" +
                    "      =#@@@*  @@#-:+%@@@- :+  *@@%+::*@@  -@@@@@:    " + "Total memory:                " + Filesystem.Utils.ConvertSize(Memory.TotalMemory * 1024 * 1024) + "\n" +
                    "           :@@@@@@       +@@@.      @@@@@@@          " + "Used memory:                 " + Filesystem.Utils.ConvertSize(Memory.GetUsedMemory() * 1024 * 1024) + " (" + WinttOS.MemoryManager.UsedPercentage + "%)" + "\n" +
                    "        .:-==*@@@@@@+. =@+*%-@*  .+@@@@@@#=          " + "Free memory:                 " + Filesystem.Utils.ConvertSize(Memory.GetFreeMemory() * 1024 * 1024) + " (" + WinttOS.MemoryManager.FreePercentage + "%)" + "\n" +
                    "      -@@@@@-       *@@   -*   %@%        @@@@@@     " + "Processor:                   " + Kernel.CpuBrandName + (Kernel.CpuClockSpeed > 0 ? "@" + Kernel.CpuClockSpeed.ToString() : "")  + "\n" +
                    "        #@@@@@@@@%@= ==   +@   .# :@+-#@@@@@@@-      " + "Screen mode:                 " + (FullScreenCanvas.IsInUse ? FullScreenCanvas.GetCurrentFullScreenCanvas().Name() : "VGA") + "\n" +
                    "          .   @@@=   @   @@%@*  @=  -@@@:            " + "Resolution:                  " + (FullScreenCanvas.IsInUse ? FullScreenCanvas.GetCurrentFullScreenCanvas().Mode.ToString() : "640x480@8") + "\n" +
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
            catch (Exception e)
            {
                Logger.DoOSLog("[Error] Sysinfo exception -> " + e.Message);
                //return new(this, ReturnCode.CRASH, e.Message);
                throw;
            }
        }
    }
}
