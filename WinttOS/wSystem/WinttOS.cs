using Cosmos.Core.Memory;
using Cosmos.HAL;
using Cosmos.System.Coroutines;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Processing;
using WinttOS.wSystem.Services;
using WinttOS.wSystem.Users;
using WinttOS.wSystem.Shell;
using WinttOS.wSystem.Shell.Commands.Misc;
using Sys = Cosmos.System;
using WinttOS.Core.Utils.Kernel;
using Cosmos.System.Graphics;
using WinttOS.Core;
using WinttOS.wSystem.Shell.bash;
using System.IO;

namespace WinttOS.wSystem
{
    public class WinttOS
    {
        #region Fields

        private static WinttOS instance => new();


        public static readonly string WinttVersion = "WinttOS v1.0.0 dev.";
        public static readonly string WinttRevision = VersionInfo.revision;

        public static WinttServiceManager ServiceManager { get; private set; } = null;
        public static UsersManager UsersManager { get; private set; } = new(null);
        public static ProcessManager ProcessManager { get; private set; } = new();
        public static CommandManager CommandManager { get; private set; } = new();
        public static PackageManager PackageManager { get; private set; }
        public static Memory MemoryManager { get; private set; } = new();
        public static bool IsSleeping { get; set; } = false;
        private static List<Action> OnSystemSleep = new();


        private static WinttServiceManager serviceManager = new();
        private static uint serviceProviderProcessID;


        public static Canvas SystemCanvas { get; private set; }

        #endregion

        #region Methods

        public static void InitializeSystem()
        {
            
            WinttCallStack.RegisterCall(new("WinttOS.Sys.WinttOS.InitializeSystem()",
                "void()", "WinttOS.cs", 40));
            try
            {
                Kernel.OnKernelFinish.Add(SystemFinish);
                InitNetwork();
                InitUsers();
                InitServiceProvider();

                ProcessManager.TryRegisterProcess(serviceManager, out serviceProviderProcessID);
                
                PackageManager = new();
                PackageManager.Initialize();

                Process srv;
                if(!ProcessManager.TryGetProcessInstance(out srv, serviceProviderProcessID))
                {
                    Kernel.WinttRaiseHardError(WinttStatus.PHASE1_INITIALIZATION_FAILED, instance, HardErrorResponseOption.OptionShutdownSystem);
                }

                ServiceManager = (WinttServiceManager)srv;

                ServiceManager.AddService(CommandManager);

                CommandManager.RegisterCommand(new DevModeCommand(new string[] { "dev-mode" }));
                CommandManager.RegisterCommand(new CommandAction(new string[] { "crash" }, User.AccessLevel.Administrator, () =>
                {
                    WinttCallStack.RegisterCall(new("WinttOS.wSystem,WinttOS.Execute()"));

                    Kernel.WinttRaiseHardError(WinttStatus.MANUALLY_INITIATED_CRASH, instance,
                        HardErrorResponseOption.OptionShutdownSystem);

                    WinttCallStack.RegisterReturn();
                }));

                ProcessManager.TryStartProcess(serviceProviderProcessID);
            }
            catch(Exception e)
            {
                WinttDebugger.Error(e.Message, true, instance);
                Kernel.WinttRaiseHardError(WinttStatus.PHASE1_INITIALIZATION_FAILED, instance, HardErrorResponseOption.OptionShutdownSystem);
            }

            Heap.Collect();

            if(File.Exists(@"0:\startup.sh"))
            {
                BashInterpreter bash = new();
                bash.Parse(@"0:\startup.sh");
                bash.Execute();
            }

            CoroutinePool.Main.PerformHeapCollection = false;

            CoroutinePool.Main.AddCoroutine(new(SystemThread()));

            CoroutinePool.Main.AddCoroutine(new(GarbageCollector()));

            CoroutinePool.Main.AddCoroutine(new(ProcessManager.UpdateProcesses()));
            
            Console.Clear();
            try
            {
                CoroutinePool.Main.StartPool();
            }
            catch(Exception e)
            {
                WinttDebugger.Error(e.Message, true);
                Kernel.WinttRaiseHardError(WinttStatus.TRAP_CAUSE_UNKNOWN, instance, 
                    HardErrorResponseOption.OptionShutdownSystem);
            }

            WinttCallStack.RegisterReturn();
        }

        private static void InitServiceProvider()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.WinttOS.InitServiceProvider()",
                "void()", "WinttOS.cs", 75));
            serviceManager.Initialize();

            serviceManager.AddService(new PowerManagerService());

            WinttCallStack.RegisterReturn();
        }

        private static void InitUsers()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.WinttOS.InitUSers()",
                "void()", "WinttOS.cs", 86));
            if (!UsersManager.TryLoadUsersData())
            {
                UsersManager.AddUser(new User.UserBuilder().SetUserName("root")
                                                           .SetAccess(User.AccessLevel.Administrator)
                                                           .Build());
                UsersManager.LoginIntoUserAccount("root", null);
            }
            if(UsersManager.RootUser.HasPassword)
            {
                tryMore:
                Console.Write("Please enter password from _user 'root': ");
                if (!UsersManager.LoginIntoUserAccount("root", Console.ReadLine()))
                    goto tryMore;

            }
            WinttCallStack.RegisterReturn();
        }

        private static void InitNetwork()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.WinttOS.InitNetwork()",
                "void()", "WinttOS.cs", 100));
            Console.WriteLine("NOTE! If you have more then one network adapters, please remove all except one!\n");

            ShellUtils.PrintTaskResult("Discovering IP address", ShellTaskResult.DOING);
            ShellUtils.MoveCursorUp();

            if (NetworkDevice.Devices.Count != 0)
            {
                using var xClient = new DHCPClient();
                try
                {
                    if (xClient.SendDiscoverPacket() != -1)
                        ShellUtils.PrintTaskResult("Discovering IP address", ShellTaskResult.OK);
                    else
                        ShellUtils.PrintTaskResult("Discovering IP address", ShellTaskResult.FAILED);

                    xClient.Close();
                }
                catch (Exception e)
                {
                    WinttDebugger.Error(e.Message, true, instance);
                    throw;
                }
            }
            else
                ShellUtils.PrintTaskResult("Discovering IP address", ShellTaskResult.FAILED);
            WinttCallStack.RegisterReturn();
        }

        public static void SystemSleep()
        {
            foreach(Action eventHandler in OnSystemSleep)
            {
                eventHandler();
            }
        }
        public static void SystemFinish()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.WinttOS.SystemFinish()",
                "void()", "WinttOS.cs", 131));
            CoroutinePool.Main.AddCoroutine(new(FinishOS()));
            WinttCallStack.RegisterReturn();
        }

        private static IEnumerator<CoroutineControlPoint> FinishOS()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.WinttOS.FinishOS()",
                "IEnumerator<CoroutineControlPoint>()", "WinttOS.cs", 139));

            WinttDebugger.Trace("FinishOS coroutine executed! Waiting 3 seconds!", instance);

            WinttCallStack.RegisterReturn();

            yield return WaitFor.Seconds(3);

            WinttCallStack.RegisterCall(new("WinttOS.Sys.WinttOS.FinishOS()",
                "IEnumerator<CoroutineControlPoint>()", "WinttOS.cs", 148));

            WinttDebugger.Trace("3 seconds elapsed, finishing running coroutines!", instance);
            foreach (var coroutine in CoroutinePool.Main.RunningCoroutines)
            {
                coroutine.Stop();
            }
            WinttDebugger.Info("Finishing Kernel!", instance);
            Console.WriteLine("Is now safe to turn off your computer!");
            if (Kernel.IsRebooting)
                Sys.Power.Reboot();
            else
                Sys.Power.Shutdown();
        }

        public static IEnumerator<CoroutineControlPoint> SystemThread()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.WinttOS.SystemThread()",
                "void()", "WinttOS.cs", 165));
            if(File.Exists(@"0:\startup.sh"))
            {
                
            }
            while (!Kernel.IsFinishingKernel)
            {
                foreach (var process in ProcessManager.Processes)
                {
                    if (process.IsProcessCritical && !process.IsProcessRunning)
                    {
                        if (process.CurrentSet == API.PrivilegesSystem.PrivilegesSet.NONE || process.HasOwnerProcess)
                            continue;
                        WinttDebugger.Error($"Critical process died => {process.ProcessName}", true, instance);
                        Kernel.WinttRaiseHardError(WinttStatus.CRITICAL_PROCESS_DIED, instance, HardErrorResponseOption.OptionShutdownSystem);
                    }
                }

                MemoryManager.Monitor();

                WinttCallStack.RegisterReturn();

                yield return WaitFor.Seconds(3);

                WinttCallStack.RegisterCall(new("WinttOS.Sys.WinttOS.SystemThread()",
                "void()", "WinttOS.cs", 165));
            }

            yield return WaitFor.Seconds(2);
        }

        public static IEnumerator<CoroutineControlPoint> GarbageCollector()
        { 
            ProcessManager.RunProcessGC();
            ServiceManager.RunServiceGC();

            WinttDebugger.Trace("Heap -> Collected: " + Heap.Collect() + " objects");

            yield return WaitFor.Milliseconds(1000);

        }

        #endregion
    }
}
