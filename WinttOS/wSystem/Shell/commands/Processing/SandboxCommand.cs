﻿using System.Collections.Generic;
using WinttOS.wSystem.Shell.Programs;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Processing
{
    internal sealed class SandboxCommand : Command
    {
        public SandboxCommand(string[] name) : base(name, AccessLevel.Default)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            uint PID;

            if (!WinttOS.ProcessManager.TryRegisterProcess(new Sandbox(arguments[0]), out PID))
            {
                return new(this, ReturnCode.ERROR);
            }

            if (!WinttOS.ProcessManager.TryStartProcess(PID))
            {
                return new(this, ReturnCode.ERROR);
            }

            return new(this, ReturnCode.OK);
        }

    }
}
