using System;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell
{
    public sealed class CommandAction : Command
    {
        private Action _action;
        public CommandAction(string[] commandValues, User.AccessLevel requiredAccess, Action action) : base(commandValues, requiredAccess)
        {
            _action = action;
        }
        public override ReturnInfo Execute()
        {
            _action();
            return new(this, ReturnCode.OK);
        }
    }
}
