using Cosmos.System.FileSystem.Listing;
using System;
using WinttOS.Core;
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

                Console.WriteLine($"<DIR>  {GlobalData.CurrentDirectory}");

                foreach (var file in dir_files)
                {
                    if (file.mEntryType == DirectoryEntryTypeEnum.File)
                    {
                        Console.WriteLine($"<FILE>\t{file.mName}\t{file.mSize}");
                    }
                    else if (file.mEntryType == DirectoryEntryTypeEnum.Directory)
                    {
                        if (file.mName.StartsWith('.'))
                            continue;
                        Console.WriteLine($"<DIR>\t{file.mName}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString() + "\n" + e.Message);
                Console.WriteLine("No files in directory");
            }

            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("- dir");
        }
    }
}
