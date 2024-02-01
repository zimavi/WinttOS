using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.wSystem.Shell.bash
{
    public class BashCallFrame
    {
        public readonly int CallLine;
        public readonly BashFunction Function;

        public BashCallFrame(int callLine, BashFunction function)
        {
            CallLine = callLine;
            Function = function;
        }
    }
}
