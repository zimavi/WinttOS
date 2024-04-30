using System.Collections.Generic;

namespace WinttOS.wSystem.Shell.bash
{
    public sealed class BashCommand
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
