using System;
using System.IO;
using WinttOS.Core;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands.FileSystem
{
    public class makefileCommand : Command
    {
        public makefileCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.addCommandUsageStrToManager(@"mkfile <new.file> - creates new file");
        }

        public override string Execute(string[] arguments)
        {
            if (arguments.Length >= 1)
            {
                var file_stream = File.Create(@"0:\" + GlobalData.CurrentDirectory + @"\" + string.Join(' ', arguments));
            }
            else
            {
                Console.Write("Enter file name: ");
                string file = Console.ReadLine();
                // Added replacment of spaces in names into _ for preventing unopenable files
                var file_stream = File.Create(@"0:\" + GlobalData.CurrentDirectory + @"\" + string.Join('\n', file.Split(' ')));
            }

            return "Created file!";
        }
    }
}
