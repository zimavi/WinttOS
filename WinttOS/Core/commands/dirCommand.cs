using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.System.FileSystem.Listing;
using WinttOS.Core.Utils.Commands;

namespace WinttOS.Core.commands
{
    public class dirCommand : Command
    {
        public dirCommand(string name) : base(name, Users.User.AccessLevel.Guest) 
        {
            HelpCommandManager.addCommandUsageStrToManager(@"dir - get list of all directories and files");
            manual = new List<string>()
            {
                "Dir command is used for checking what is in current directory.",
                "When you run this command, you will see some lines.",
                "First is going to be '<DIR> 0:\\whatever\\folder' what means",
                "that we are looking files and folders in '0:\\whatever\\folder'",
                "Then we can see both '<FILE> whatever.name   size in MB' and",
                "'<DIR> whatever_name' which shows us is that a file or folder."
            };
        }

        public override string execute(string[] arguments)
        {
            try
            {
                var dir_files = GlobalData.fs.GetDirectoryListing(@"0:\" + GlobalData.currDir);

                Console.WriteLine($"<DIR>  0:\\{GlobalData.currDir}");

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
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString() + "\n" + e.Message);
                Console.WriteLine("No files in directory");
            }

            return String.Empty;
        }
    }
}
