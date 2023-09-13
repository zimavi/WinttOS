using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WinttOS.Core.Users;

namespace WinttOS.Core
{
    public abstract class Command
    {
        public readonly string name;
        public List<string> manual { get; protected set; }

        public readonly bool isHidden;

        public readonly User.AccessLevel RequiredAccessLevel;

        public Command(string name)
        {
            this.name = name;
            manual = new();
            isHidden = false;
            RequiredAccessLevel = User.AccessLevel.Default;
        }
        public Command(string name, User.AccessLevel requiredAccessLevel)
        {
            this.RequiredAccessLevel = requiredAccessLevel;
            this.name = name;
            manual = new();
            isHidden = false;
        }
        public Command(string name, bool hidden)
        {
            RequiredAccessLevel = User.AccessLevel.Default;
            this.name = name;
            manual = new List<string>();
            isHidden = hidden;
        }
        public Command(string name, bool hidden, User.AccessLevel requiredAccessLevel)
        {
            RequiredAccessLevel = requiredAccessLevel;
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
