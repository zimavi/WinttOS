using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Base
{
    public abstract class Command
    {
        public readonly string name;
        public List<string> manual { get; protected set; }

        public readonly bool isHidden;

        public Command(string name, bool hidden)
        {
            this.name = name;
            manual = new List<string>();
            isHidden = hidden;
        }

        public virtual string execute(string[] arguments)
        {
            return "";
        }
    }
}
