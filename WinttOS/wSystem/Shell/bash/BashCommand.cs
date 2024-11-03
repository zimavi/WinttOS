using System.Collections.Generic;

namespace WinttOS.wSystem.Shell.bash
{
    public sealed class BashCommand
    {
        public string Name;
        public int Line;
        public List<string> Args;
        public bool IsConditional;
        public int? ConditionalEnd;
        public int? ElseLine;
        public bool IsLoop;
        public int? LoopEnd;
        public bool IsLoopEnd;
        public bool IsFunctionCommand;

        public BashCommand(string name, int line, List<string> args, bool isConditional = false, int? conditionalEnd = null, int? elseLine = null, bool isLoop = false, int? loopEnd = null, bool isLoopEnd = false, bool isFunctionCommand = false)
        {
            Name = name;
            Line = line;
            Args = args;
            IsConditional = isConditional;
            ConditionalEnd = conditionalEnd;
            ElseLine = elseLine;
            IsLoop = isLoop;
            LoopEnd = loopEnd;
            IsLoopEnd = isLoopEnd;
            IsFunctionCommand = isFunctionCommand;
        }
    }
}
