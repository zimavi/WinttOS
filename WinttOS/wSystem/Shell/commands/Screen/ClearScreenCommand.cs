using System;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Screen
{
    public sealed class ClearScreenCommand : Command
    {
        public ClearScreenCommand(string[] name) : base(name, User.AccessLevel.Guest)
        { }
        public override ReturnInfo Execute()
        {
            if (WinttOS.IsTty)
                WinttOS.Tty.ClearText();
            else
                Console.Clear();
            return new(this, ReturnCode.OK);
        }
    }
}
