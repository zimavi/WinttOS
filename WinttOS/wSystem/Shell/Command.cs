using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell
{
    public enum CommandType
    {
        Filesystem,
        Network,
        Utils,
        Unknown
    }

    public enum ReturnCode
    {
        ERROR_ARG,
        ERROR,
        CRASH,
        OK
    }
    public class ReturnInfo
    {
        private Command _command;
        internal Command Command => _command;

        private ReturnCode _code;
        internal ReturnCode Code => _code;

        private string _info;
        public string Info => _info;

        public ReturnInfo(Command command, ReturnCode code, string info = "Unknown error.")
        {
            _command = command;
            _code = code;
            _info = info;
        }
    }

    public abstract class Command
    {
        public string[] CommandValues;
        public string Description { get; set; }
        public CommandType Type { get; set; }
        public List<string> CommandManual { get; protected set; }

        public readonly bool IsHiddenCommand;

        public readonly User.AccessLevel RequiredAccessLevel;

        public Command(string[] commandValues) : this(commandValues, false, User.AccessLevel.Default)
        { }

        public Command(string[] commandValues, User.AccessLevel requiredAccessLevel) : this(commandValues, false, requiredAccessLevel)
        { }

        public Command(string[] commandValues, bool hidden) : this(commandValues, hidden, User.AccessLevel.Default)
        { }

        public Command(string[] commandValues, bool hidden, User.AccessLevel requiredAccessLevel)
        {
            RequiredAccessLevel = requiredAccessLevel;
            CommandValues = commandValues;
            CommandManual = new List<string>();
            IsHiddenCommand = hidden;
        }

        public virtual ReturnInfo Execute()
        {
            return new ReturnInfo(this, ReturnCode.ERROR_ARG);
        }

        public virtual ReturnInfo Execute(List<string> arguments)
        {
            return new ReturnInfo(this, ReturnCode.ERROR_ARG);
        }

        public virtual void PrintHelp()
        {
            Console.WriteLine("No help information for this command!");
        }


        public bool ContainsCommand(string command)
        {
            foreach (string commandvalue in CommandValues)
            {
                if (commandvalue == command)
                {
                    return true;
                }
            }
            return false;
        }

        public string CommandStarts(string cMDToComplete)
        {
            foreach (string value in CommandValues)
            {
                if (value.StartsWith(cMDToComplete))
                {
                    return value;
                }
            }
            return null;
        }
    }
}
