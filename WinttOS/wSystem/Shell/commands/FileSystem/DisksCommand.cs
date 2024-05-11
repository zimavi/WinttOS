using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Shell.commands.FileSystem
{
    public class DisksCommand : Command
    {
        public DisksCommand(string[] commandValues) : base(commandValues)
        { }


        public override ReturnInfo Execute()
        {

            return new(this, ReturnCode.OK);
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
        }
    }
}
