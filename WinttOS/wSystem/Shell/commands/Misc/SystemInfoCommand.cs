using Cosmos.Core;
using Cosmos.System.Graphics;
using System;
using System.Drawing;
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

                string[] infoLines = new string[]
                    {
                        "                          =@                         ",
                        "                          @@@                        ",
                        "                  =@@@*. @@@@% =#@@@                 ",
                        "              +%   -@@@@  @@+ @@@@@    #.            ",
                        "              @@@   =@@@@*=*.@@@@@    @@@            " + UsersManager.userLogged + "@wintt-pc",
                        "             #@@@@%-@#  :@@@@@   @@:=@@@@:           " + "-------------",
                        "       =@@@@# @@@ -@@@@+  *@  :@@@@% %@@% -+*%@-     " + "OS:                      WinttOS",
                        "        -@@@@+ @@  @@@@@  :+  @@@@@% =@+ #@@@@+      " + "Kernel:                  Cosmos-devkit",
                        "          **+*@@@=  %@#.@=-+ @+:@@.  @@%@@@@@.       " + ".NET version:            6.0",
                        "        .@@@%@@@@@=  +-   @@*   @   #@@@@+:#@:       " + "OS version:              " + WinttOS.WinttVersion,
                        "       @@@@@@      +@%@   :+   -##@-    :@@@@@@      " + "OS build:                " + WinttOS.WinttRevision,
                        "      =#@@@*  @@#-:+%@@@- :+  *@@%+::*@@  -@@@@@:    " + "Date and time:           " + Time.DayString() + "/" + Time.MonthString() + "/" + Time.YearString() + ", " + Time.TimeString(true, true, true),
                        "           :@@@@@@       +@@@.      @@@@@@@          " + "Uptime:                  " + WinttOS.Uptime.ToString(),
                        "        .:-==*@@@@@@+. =@+*%-@*  .+@@@@@@#=          " + "Total memory:            " + Filesystem.Utils.ConvertSize(Memory.TotalMemory * 1024 * 1024),
                        "      -@@@@@-       *@@   -*   %@%        @@@@@@     " + "Used memory:             " + Filesystem.Utils.ConvertSize(Memory.GetUsedMemory() * 1024 * 1024) + " (" + WinttOS.MemoryManager.UsedPercentage + "%)",
                        "        #@@@@@@@@%@= ==   +@   .# :@+-#@@@@@@@-      " + "Free memory:             " + Filesystem.Utils.ConvertSize(Memory.GetFreeMemory() * 1024 * 1024) + " (" + WinttOS.MemoryManager.FreePercentage + "%)",
                        "          .   @@@=   @   @@%@*  @=  -@@@:            " + "Processor:               " + Kernel.CpuBrandName + (Kernel.CpuClockSpeed > 0 ? "@" + Kernel.CpuClockSpeed.ToString() : ""),
                        "        *@@@@+ @@  #@@@@# -*  @@@@@. @@.+@@@@.       " + "Screen mode:             " + (FullScreenCanvas.IsInUse ? FullScreenCanvas.GetCurrentFullScreenCanvas().Name() : "VGA"),
                        "       @@@@@* *@@  @@@@@  -*  :@@@@+ *@# #@@@@-      " + "Resolution:              " + (FullScreenCanvas.IsInUse ? FullScreenCanvas.GetCurrentFullScreenCanvas().Mode.ToString() : "640x480@8"),
                        "             @@@@+@@@@:  +@@@   @@@@@@@@@.   :=      ",
                        "             #@@@   @  -@@@@@@@  %   @@@%            ",
                        "              @@    @@@@# *@  @@@@+   @@.            ",
                        "                   @@@@@ @@@@ -@@@@+                 ",
                        "                   .     @@@@      .                 ",
                        "                          @@                         "
                    };

                if (WinttOS.IsTty)
                {
                    foreach (string line in infoLines)
                    {
                        if (line.Contains("-------------") || line.Contains("OS:") || line.Contains("Kernel:") || line.Contains(".NET version:") ||
                            line.Contains("OS version:") || line.Contains("OS build:") || line.Contains("Date and time:") || line.Contains("Uptime:") ||
                            line.Contains("Total memory:") || line.Contains("Used memory:") || line.Contains("Free memory:") || line.Contains("Processor:") ||
                            line.Contains("Screen mode:") || line.Contains("Resolution:"))
                        {
                            WinttOS.Tty.ForegroundColor = Color.Cyan;
                        }
                        else
                        {
                            WinttOS.Tty.ForegroundColor = Color.White;
                        }

                        WinttOS.Tty.WriteNoUpdate(line + "\n");
                    }

                    WinttOS.Tty.Update();
                }
                else
                {
                    foreach (string line in infoLines)
                    {
                        if (line.Contains("-------------") || line.Contains("OS:") || line.Contains("Kernel:") || line.Contains(".NET version:") ||
                            line.Contains("OS version:") || line.Contains("OS build:") || line.Contains("Date and time:") || line.Contains("Uptime:") ||
                            line.Contains("Total memory:") || line.Contains("Used memory:") || line.Contains("Free memory:") || line.Contains("Processor:") ||
                            line.Contains("Screen mode:") || line.Contains("Resolution:"))
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                        Console.WriteLine(line);
                    }

                    Console.ResetColor();
                }
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
