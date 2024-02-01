using System;
using System.Collections.Generic;
using Cosmos.System.FileSystem.Listing;
using WinttOS.Core;
using WinttOS.wSystem.Shell.Utils.Commands;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.FileSystem
{
    public class DirCommand : Command
    {
        public DirCommand(string[] name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"dir - get list of all directories and files");
            CommandManual = new List<string>()
            {
                "Dir command is used for checking what is in current directory.",
                "When you run this command, you will see some lines.",
                "First is going to be '<DIR> 0:\\whatever\\folder' what means",
                "that we are looking files and folders in '0:\\whatever\\folder'",
                "Then we can see both '<FILE> whatever.name   size in MB' and",
                "'<DIR> whatever_name' which shows us is that a file or folder."
            };
        }

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
    }
}
