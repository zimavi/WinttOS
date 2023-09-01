using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.System.FileSystem.Listing;
using WinttOS.Base.Utils.Commands;

namespace WinttOS.Base.commands
{
    public class dirCommand : Command
    {
        public dirCommand(string name) : base(name) 
        {
            HelpCommandManager.addCommandUageStrToManager(@"dir - get list of all directories and files");
        }

        public override string execute(string[] arguments)
        {
            try
            {
                var dir_files = GlobalData.fs.GetDirectoryListing(@"0:\" + GlobalData.currDir);

                Console.WriteLine($"<DIR>  0:\\{GlobalData.currDir} ({(int)GlobalData.currDir[0]})");

                foreach (var file in dir_files)
                {
                    if (file.mEntryType == DirectoryEntryTypeEnum.File)
                        Console.WriteLine($"<FILE>\t{file.mName}\t{file.mSize}");
                    else if (file.mEntryType == DirectoryEntryTypeEnum.Directory)
                        Console.WriteLine($"<DIR>\t{file.mName}");
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
