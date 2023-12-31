﻿using System;
using System.Collections.Generic;
using Cosmos.System.FileSystem.Listing;
using WinttOS.Core;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands.FileSystem
{
    public class dirCommand : Command
    {
        public dirCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.addCommandUsageStrToManager(@"dir - get list of all directories and files");
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

        public override string Execute(string[] arguments)
        {
            try
            {
                var dir_files = GlobalData.FileSystem.GetDirectoryListing(@"0:\" + GlobalData.CurrentDirectory);

                Console.WriteLine($"<DIR>  0:\\{GlobalData.CurrentDirectory}");

                foreach (var file in dir_files)
                {
                    if (file.mEntryType == DirectoryEntryTypeEnum.File)
                        Console.WriteLine($"<FILE>\t{file.mName}\t{file.mSize}");
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

            return string.Empty;
        }
    }
}
