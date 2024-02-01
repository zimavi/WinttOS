using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.wSystem.Shell.bash
{
    public class BashCommand
    {
        public string Name;
        public int Line;
        public List<string> Args;

        public BashCommand(string name, int line, List<string> args)
        {
            Name = name;
            Line = line;
            Args = args;
        }
    }
}
