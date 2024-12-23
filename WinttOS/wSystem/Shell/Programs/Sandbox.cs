﻿using WinttOS.wSystem.Processing;

namespace WinttOS.wSystem.Shell.Programs
{
    internal sealed class Sandbox : Process
    {

        private string targetName;
        private Process target;
        private uint targetPID;

        public Sandbox(string sandboxTarget) : base($"Sandbox ({sandboxTarget})", ProcessType.Program)
        {
            targetName = sandboxTarget;
        }

        public override void Start()
        {
            base.Start();

            target = new SandboxingTest();

            SetChild(target);

            if (!TrySetChildPrivileges(target, wAPI.PrivilegesSystem.PrivilegesSet.NONE))
            {
                WinttOS.ProcessManager.TryStopProcess(ProcessID);
            }

            if (!WinttOS.ProcessManager.TryRegisterProcess(target, out targetPID))
            {
                WinttOS.ProcessManager.TryStopProcess(ProcessID);
            }

            WinttOS.ProcessManager.TryStartProcess(targetPID);
        }

        public override void Stop()
        {
            base.Stop();

            WinttOS.ProcessManager.TryStopProcess(targetPID);
        }
    }
}
