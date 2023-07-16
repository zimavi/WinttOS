using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Base.commands
{
    public class WriteFileCommand : Command
    {
        public WriteFileCommand(string name) : base(name) { }

        public override string execute(string[] arguments)
        {
            try
            {
                //File.WriteAllText(GlobalData.currDir + arguments[0])
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return String.Empty;
        }
    }
}
