using System.Collections.Generic;
using WinttOS.wSystem.Filesystem;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Shell.commands.FileSystem
{
    public class MapDirectoryCommand : Command
    {
        public MapDirectoryCommand(string[] commandValues) : base(commandValues, Users.AccessLevel.Administrator)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments.Count < 2)
                return new(this, ReturnCode.ERROR_ARG);

            IOMapper.AddMapping(arguments[0], arguments[1]);

            return new(this, ReturnCode.OK);
        }

        public override ReturnInfo Execute()
        {
            PrintHelp();
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            // Physical path is basically Cosmos path format
            SystemIO.STDOUT.PutLine("Usage: ");
            SystemIO.STDOUT.PutLine("mapdir [physical\\path] [FHS/path]");
        }
    }
}
