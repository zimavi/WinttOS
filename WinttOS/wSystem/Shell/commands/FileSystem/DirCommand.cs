using Cosmos.System.FileSystem.Listing;
using System;
using WinttOS.Core;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.FileSystem
{
    public sealed class DirCommand : Command
    {
        public DirCommand(string[] name) : base(name, User.AccessLevel.Guest)
        { }

        public override ReturnInfo Execute()
        {
            try
            {
                var dir_files = GlobalData.FileSystem.GetDirectoryListing(GlobalData.CurrentDirectory);

                SystemIO.STDOUT.PutLine($"<DIR>  {GlobalData.CurrentDirectory}");

                foreach (var file in dir_files)
                {
                    if (file.mEntryType == DirectoryEntryTypeEnum.File)
                    {
                        SystemIO.STDOUT.PutLine($"<FILE>\t{file.mName}\t{file.mSize}");
                    }
                    else if (file.mEntryType == DirectoryEntryTypeEnum.Directory)
                    {
                        if (file.mName.StartsWith('.'))
                            continue;
                        SystemIO.STDOUT.PutLine($"<DIR>\t{file.mName}");
                    }
                }
            }
            catch (Exception e)
            {
                SystemIO.STDOUT.PutLine(e.ToString() + "\n" + e.Message);
                SystemIO.STDOUT.PutLine("No files in directory");
            }

            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("- dir");
        }
    }
}
