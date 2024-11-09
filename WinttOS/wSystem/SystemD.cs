using Cosmos.Core;
using Cosmos.System.Graphics;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Processing;
using WinttOS.wSystem.Services;
using WinttOS.wSystem.Shell;
using WinttOS.wSystem.Shell.commands.Misc;
using WinttOS.wSystem.Shell.Commands.Misc;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem
{
    internal class SystemD : Process
    {

        private WinttServiceManager serviceManager;

        private uint serviceProviderProcessID;
        private uint commandManagerProcessID;
        private uint windowManagerProcessID;

        public static bool isWinMonRunning = false;

        public SystemD() : base("Systemd", ProcessType.KernelComponent)
        {

        }

        public override void Start()
        {
            base.Start();

            serviceManager = new WinttServiceManager();

            Logger.DoOSLog("[Info] Initializing command manager");

            WinttOS.CommandManager = new CommandManager();
            WinttOS.WindowManager = new();

            Logger.DoOSLog("[Info] Registering service manager -> process manager");

            WinttOS.ProcessManager.TryRegisterProcess(serviceManager, out serviceProviderProcessID);
            SetChild(serviceManager);

            if (!WinttOS.ProcessManager.TryGetProcessInstance(out Process srv, serviceProviderProcessID))
            {
                Kernel.WinttRaiseHardError("Cannot start service manager", this);
            }

            WinttOS.ServiceManager = (WinttServiceManager)srv;

            unsafe
            {
                GCImplementation.DecRootCount((ushort*)&srv);
            }

            /*
            Logger.DoOSLog("[Info] Registering window manager -> service manager");
            WinttOS.ServiceManager.AddService(WinttOS.WindowManager);
            SetChild(WinttOS.WindowManager);
            */

            
            Logger.DoOSLog("[Info] Registering command manager -> service manager");
            WinttOS.ServiceManager.AddService(WinttOS.CommandManager);
            commandManagerProcessID = WinttOS.CommandManager.ProcessID;

            WinttOS.CommandManager.RegisterCommand(new DevModeCommand(new string[] { "dev-mode" }));
            WinttOS.CommandManager.RegisterCommand(new WinManStartCommand(new string[] { "winstart" }));
            WinttOS.CommandManager.RegisterCommand(new CommandAction(new string[] { "crash" }, AccessLevel.Administrator, () =>
            {

                Kernel.WinttRaiseHardError("Manually initiated crash", this);

            }));
            WinttOS.CommandManager.RegisterCommand(new CommandAction(new string[] { "tty" }, AccessLevel.Administrator, () =>
            {

                if (WinttOS.IsTty)
                {
                    SystemIO.STDOUT = new ConsoleIO();
                    SystemIO.STDERR = new ConsoleIO();
                    SystemIO.STDIN = new ConsoleIO();

                    if (FullScreenCanvas.IsInUse)
                        FullScreenCanvas.Disable();

                    WinttOS.Tty = default;
                    WinttOS.IsTty = false;
                }
                else
                {
                    SystemIO.STDOUT = new TtyIO();
                    SystemIO.STDERR = new TtyIO();
                    SystemIO.STDIN = new TtyIO();

                    WinttOS.Tty = new(1920, 1080);
                    WinttOS.IsTty = true;
                }

            }));
            

            Logger.DoOSLog("[Info] Starting service manager");
            WinttOS.ProcessManager.TryStartProcess(serviceProviderProcessID);
        }

        public override void Stop() 
        { 
            base.Stop();

            WinttOS.ProcessManager.TryStopProcess(serviceProviderProcessID);
            WinttOS.ProcessManager.TryStopProcess(commandManagerProcessID);
            //WinttOS.ProcessManager.TryStopProcess(windowManagerProcessID);
        }

        public override void Update()
        {
            base.Update();

            Process proc;
            if (!WinttOS.ProcessManager.TryGetProcessInstance(out proc, serviceProviderProcessID))
            {
                Kernel.WinttRaiseHardError("Systemd -> Process instance not available", this);
            }
            if (!proc.IsProcessRunning)
            {
                TryToRestart(serviceProviderProcessID);
            }

            if (!isWinMonRunning)
            {
                if (!WinttOS.ProcessManager.TryGetProcessInstance(out proc, commandManagerProcessID))
                {
                    Kernel.WinttRaiseHardError("Systemd -> Process instance not available", this);
                }
                if (!proc.IsProcessRunning)
                {
                    TryToRestart(commandManagerProcessID);
                }
            }
            
            if (!WinttOS.ProcessManager.TryGetProcessInstance(out proc, windowManagerProcessID))
            {
                if (WinttOS.IsTty)
                    return;
                WinttOS.IsTty = true;
                WinttOS.Tty = new(1920, 1080);

                ShellUtils.PrintTaskResult("Systemd", ShellTaskResult.FAILED, "WinMon instance not available");
            }
            if (!proc.IsProcessRunning)
            {
                TryToRestart(windowManagerProcessID);
            }
        }

        private void TryToRestart(uint ProcessID)
        {
            Logger.DoOSLog("[Warn] Systemd -> Process died, trying to restart!");

            for (int i = 0; i < 3; i++)
            {
                if (WinttOS.ProcessManager.TryStartProcess(ProcessID))
                {
                    return;
                }
            }

            Kernel.WinttRaiseHardError("Systemd -> Unable to restart process", this);
        }
    }
}
