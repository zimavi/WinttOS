using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.System.Users;

namespace WinttOS.System.wosh
{
    public abstract class Command
    {
        public readonly string CommandName;
        public List<string> CommandManual { get; protected set; }

        public readonly bool IsHiddenCommand;

        public readonly User.AccessLevel RequiredAccessLevel;

        public Command(string name)
        {
            CommandName = name;
            CommandManual = new();
            IsHiddenCommand = false;
            RequiredAccessLevel = User.AccessLevel.Default;
        }
        public Command(string name, User.AccessLevel requiredAccessLevel)
        {
            RequiredAccessLevel = requiredAccessLevel;
            CommandName = name;
            CommandManual = new();
            IsHiddenCommand = false;
        }
        public Command(string name, bool hidden)
        {
            RequiredAccessLevel = User.AccessLevel.Default;
            CommandName = name;
            CommandManual = new List<string>();
            IsHiddenCommand = hidden;
        }
        public Command(string name, bool hidden, User.AccessLevel requiredAccessLevel)
        {
            RequiredAccessLevel = requiredAccessLevel;
            CommandName = name;
            CommandManual = new List<string>();
            IsHiddenCommand = hidden;
        }

        public virtual string Execute(string[] arguments)
        {
            return "";
        }
    }
}
