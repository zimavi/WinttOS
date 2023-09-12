using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils.Commands;

namespace WinttOS.Base.commands
{
    public class makedirCommand : Command
    {
        public makedirCommand(string name) : base(name, Users.User.AccessLevel.Guest) 
        {
            HelpCommandManager.addCommandUsageStrToManager(@"mkdir <dir> - creates new directory in current one");
        }

        public override string execute(string[] arguments)
        {
            if(arguments.Length >= 1)
            {
                GlobalData.fs.CreateDirectory(@"0:\" + GlobalData.currDir + arguments[0]);
            }
            else
            {
                Console.Write("Enter new dir name: ");
                string dir = Console.ReadLine();
                // Added replacment of spaces in names into _ for preventing unopenable folders
                GlobalData.fs.CreateDirectory(@"0:\" + GlobalData.currDir + string.Join('_', dir.Split(' ')));
            }
            return "Created directory!";
        }
    }
}
