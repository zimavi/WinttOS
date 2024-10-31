using Cosmos.System.FileSystem;
using System.Collections.Generic;
using WinttOS.Core;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Shell.Utils;

namespace WinttOS.wSystem.Shell.commands.FileSystem
{
    public class DisksCommand : Command
    {
        public DisksCommand(string[] commandValues) : base(commandValues)
        {
        }


        public override ReturnInfo Execute() => ListVolumes();

        /*
         *  disks [--list-volumes      | -lv]
         *  disks [--change-volume     | -c ]
         *  disks [--list-partitions   | -lp]
         *  disks [--format-partition  | -fp]
         *  disks [--make-partition    | -mp]
         *  disks [--delete-partition  | -dp]
         *  disks [--disk-info         | -di]
         */
        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments[0] == "--list-volumes" || arguments[0] == "-lv")
                return ListVolumes();
            else if (arguments[0] == "--change-volume" || arguments[0] == "-c")
                return ChangeVolume(arguments[1]);
            else if (arguments[0] == "--list-partitions" || arguments[0] == "-lp")
                return ListDisks();
            else if (arguments[0] == "--format-partition" || arguments[0] == "-fp")
                return FormatVolume(int.Parse(arguments[1]), int.Parse(arguments[2]));
            else if (arguments[0] == "--make-partition" || arguments[0] == "-mp")
                return MakeDisk(int.Parse(arguments[1]), int.Parse(arguments[2]));
            else if (arguments[0] == "--delete-partition" || arguments[0] == "-dp")
                return DeleteDisk(int.Parse(arguments[1]), int.Parse(arguments[2]));
            else if (arguments[0] == "--disk-info" || arguments[0] == "-di")
                return DiskInfo(int.Parse(arguments[1]));
            return new(this, ReturnCode.ERROR_ARG);
        }

        public ReturnInfo DeleteDisk(int disknum, int idx)
        {
            Disk disk = null;
            int i = 0;

            idx--;

            foreach (var d in GlobalData.FileSystem.GetDisks())
            {
                if (i == disknum)
                {
                    disk = d;

                    Logger.DoOSLog("[Info] Deleting partition #" + (idx + 1) + " on disk #" + disknum);
                    disk.DeletePartition(idx);

                    SystemIO.STDOUT.PutLine("Partition #" + (idx + 1) + " has been deleted from disk #" + disknum);

                    RemountDisks();

                    break;
                }

                i++;
            }

            if (disk == null)
            {
                return new(this, ReturnCode.ERROR, "Failed to find the drive");
            }

            return new(this, ReturnCode.OK);
        }

        public ReturnInfo MakeDisk(int disknum, int size)
        {
            Disk disk = null;
            int i = 0;

            foreach (var d in GlobalData.FileSystem.GetDisks())
            {
                if (i == disknum)
                {
                    disk = d;

                    Logger.DoOSLog("[Info] Creating partition on disk #" + disknum);
                    disk.CreatePartition(size);

                    SystemIO.STDOUT.PutLine("Partition has been created on disk #" + disknum);

                    RemountDisks();

                    break;
                }

                i++;
            }

            if (disk == null)
            {
                return new(this, ReturnCode.ERROR, "Failed to find the drive");
            }

            return new(this, ReturnCode.OK);
        }

        public ReturnInfo DiskInfo(int disknumber)
        {
            Disk disk = null;
            int i = 0;

            foreach (var d in GlobalData.FileSystem.GetDisks())
            {
                if (i == disknumber)
                {
                    disk = d;

                    disk.DisplayInformation();

                    break;
                }

                i++;
            }
            if (disk == null)
            {
                return new ReturnInfo(this, ReturnCode.ERROR, "Failed to find the drive.");
            }

            return new ReturnInfo(this, ReturnCode.OK);
        }

        public ReturnInfo ListDisks()
        {
            int i = 0;

            foreach(var disk in GlobalData.FileSystem.GetDisks())
            {
                string type = disk.IsMBR ? "MBR" : "GPT";

                SystemIO.STDOUT.PutLine("");
                SystemIO.STDOUT.PutLine("Disk #" + i + " (" + type + ")");

                disk.DisplayInformation();

                i++;
            }

            return new(this, ReturnCode.OK);
        }

        public ReturnInfo FormatVolume(int drivenum, int partition)
        {
            Disk disk = null;
            int i = 0;

            partition--;

            foreach(var d in GlobalData.FileSystem.GetDisks())
            {
                if(i == drivenum)
                {
                    disk = d;

                    Logger.DoOSLog("[Info] Formatting partition #" + (partition + 1) + " with FAT32 on drive #" + drivenum);
                    disk.FormatPartition(partition, "FAT32", true);

                    SystemIO.STDOUT.PutLine("Partition #" + (partition + 1) + " has been formatte to FAT32 on disk #" + drivenum);

                    RemountDisks();

                    break;
                }

                i++;
            }

            if(disk == null)
            {
                return new(this, ReturnCode.ERROR, "Failed to find the drive");
            }

            return new(this, ReturnCode.OK);
        }

        public ReturnInfo ChangeVolume(string volume)
        {
            try
            {
                bool exist = false;

                foreach(var vol in GlobalData.FileSystem.GetVolumes())
                {
                    if(vol.mName == volume + ":\\")
                    {
                        exist = true;
                        GlobalData.CurrentVolume = vol.mName;
                        GlobalData.CurrentDirectory = GlobalData.CurrentVolume;

                        break;
                    }
                }
                if (!exist)
                {
                    return new(this, ReturnCode.ERROR, "The specified drive is not found");
                }
            }
            catch
            {
                return new(this, ReturnCode.ERROR, "The specified drive is not found");
            }

            return new(this, ReturnCode.OK);
        }

        public ReturnInfo ListVolumes()
        {
            var vols = GlobalData.FileSystem.GetVolumes();

            var fmt = new ConsoleColumnFormatter(20, 4);

            fmt.Write("   Volume");
            fmt.Write("Format");
            fmt.Write("Size");
            fmt.Write("Used space");

            foreach (var vol in vols)
            {
                if(vol.mName == GlobalData.CurrentVolume && vols.Count > 1)
                {
                    fmt.Write(" > " + vol.mName);
                }
                else
                {
                    fmt.Write("   " + vol.mName);
                }

                long sizeTotal = GlobalData.FileSystem.GetTotalSize(vol.mName);

                fmt.Write(GlobalData.FileSystem.GetFileSystemType(vol.mName));
                fmt.Write(Filesystem.Utils.ConvertSize(sizeTotal));
                fmt.Write(Filesystem.Utils.ConvertSize(sizeTotal - GlobalData.FileSystem.GetTotalFreeSpace(vol.mName)));
            }

            return new(this, ReturnCode.OK);
        }

        public void RemountDisks()
        {
            Logger.DoOSLog("[Info] Remounting disks in result of changes");
            GlobalData.FileSystem.Disks.Clear();
            GlobalData.FileSystem.Initialize(false);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("disks");
            SystemIO.STDOUT.PutLine("disks [--list-volumes      | -lv]");
            SystemIO.STDOUT.PutLine("disks [--change-volume     | -c ] [volume]");
            SystemIO.STDOUT.PutLine("disks [--list-partitions   | -lp]");
            SystemIO.STDOUT.PutLine("disks [--format-partition  | -fp] [partition number]");
            SystemIO.STDOUT.PutLine("disks [--make-partition    | -mp] [size]");
            SystemIO.STDOUT.PutLine("disks [--delete-partition  | -dp] [partition number]");
            SystemIO.STDOUT.PutLine("disks [--disk-info         | -di] [partition number]");
        }
    }
}
