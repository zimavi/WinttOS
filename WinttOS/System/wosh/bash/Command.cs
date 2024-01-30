using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.System.wosh.bash
{
    internal class Command
    {
        private Interpreter interpreter;

        public Command(Interpreter interpreter)
        {
            this.interpreter = interpreter;
        }

    }
}
