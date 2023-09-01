using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils.Commands;

namespace WinttOS.Base.commands
{
    public class makefileCommand : Command
    {
        public makefileCommand(string name) : base(name) 
        {
            HelpCommandManager.addCommandUageStrToManager(@"mkfile <new.file> - creates new file");
        }

        public override string execute(string[] arguments)
        {
            if (arguments.Length >= 1)
            {
                var file_stream = File.Create(@"0:\" + GlobalData.currDir + @"\" + String.Join(' ', arguments));
            }
            else
            {
                Console.Write("Enter file name: ");
                string file = Console.ReadLine();

                var file_stream = File.Create(@"0:\" + GlobalData.currDir + @"\" + file);
            }

            return "Created file!";
        }
    }
}
