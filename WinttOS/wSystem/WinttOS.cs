using Cosmos.Core;
using Cosmos.Core.Memory;
using Cosmos.HAL;
using Cosmos.System.Coroutines;
using Cosmos.System.Graphics;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using System;
using System.Collections.Generic;
using System.IO;
using WinttOS.Core;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Kernel;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.GUI;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Processing;
using WinttOS.wSystem.Scheduling;
using WinttOS.wSystem.Services;
using WinttOS.wSystem.Shell;
using WinttOS.wSystem.Shell.Commands.Misc;
using WinttOS.wSystem.Users;
using WinttOS.wSystem.wAPI.Events;
using WinttOS.wSystem.wAPI.PrivilegesSystem;
using Sys = Cosmos.System;


namespace WinttOS.wSystem
{

    public class WinttOS
    {

        #region Fields

        private static WinttOS instance => new();

        public static PrivilegesSet CurrentExecutionSet { get; internal set; } = PrivilegesSet.HIGHEST;

        public static readonly string WinttVersion = "WinttOS v1.1.0 dev.";
        public static readonly string WinttRevision = VersionInfo.revision;

        public static WinttServiceManager ServiceManager { get; private set; }
        public static TaskScheduler SystemTaskScheduler { get; private set; }
        public static UsersManager UsersManager { get; /*private */set; }
        public static ProcessManager ProcessManager { get; private set; }
        public static CommandManager CommandManager { get; private set; }
        public static PackageManager PackageManager { get; private set; }
        public static Memory MemoryManager { get; private set; }

        public static bool IsTty { get; set; } = false;
        public static Tty Tty { get; set; }
        public static bool IsSleeping { get; set; } = false;
        public static bool KernelPrint { get; private set; } = true;
        private static List<Action> OnSystemSleep;


        private static WinttServiceManager serviceManager;
        private static uint serviceProviderProcessID;

        private static EventBus _globalEventBus;


        public static Canvas SystemCanvas { get; private set; }

        #endregion

        #region Methods

        public static void InitializeSystem()
        {
            
            WinttCallStack.RegisterCall(new("WinttOS.Sys.WinttOS.InitializeSystem()",
                "void()", "WinttOS.cs", 40));
            try
            {
                ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.NONE, "Standard IO");

                SystemIO.STDOUT = new TtyIO();
                SystemIO.STDERR = new TtyIO();
                SystemIO.STDIN = new TtyIO();

                ShellUtils.PrintTaskResult("Starting", ShellTaskResult.NONE, "TTY");

                Tty = new(1920, 1080);
                IsTty = true;

                SystemTaskScheduler = new TaskScheduler();
                UsersManager = new UsersManager(null);
                ProcessManager = new ProcessManager();
                serviceManager = new WinttServiceManager();
                CommandManager = new CommandManager();
                PackageManager = new PackageManager();
                MemoryManager = new Memory();

                serviceManager.Initialize();
                PackageManager.Initialize();

                OnSystemSleep = new List<Action>();

                Kernel.OnKernelFinish.Add(SystemFinish);
                InitNetwork();
                InitUsers();

                ProcessManager.TryRegisterProcess(serviceManager, out serviceProviderProcessID);

                Process srv;
                if(!ProcessManager.TryGetProcessInstance(out srv, serviceProviderProcessID))
                {
                    Kernel.WinttRaiseHardError(WinttStatus.PHASE1_INITIALIZATION_FAILED, instance, HardErrorResponseOption.OptionShutdownSystem);
                }

                ServiceManager = (WinttServiceManager)srv;

                unsafe
                {
                    GCImplementation.DecRootCount((ushort*)&srv);
                }

                ServiceManager.AddService(CommandManager);

                CommandManager.RegisterCommand(new DevModeCommand(new string[] { "dev-mode" }));
                CommandManager.RegisterCommand(new CommandAction(new string[] { "crash" }, User.AccessLevel.Administrator, () =>
                {
                    WinttCallStack.RegisterCall(new("WinttOS.wSystem.WinttOS.Execute()"));

                    Kernel.WinttRaiseHardError(WinttStatus.MANUALLY_INITIATED_CRASH, instance,
                        HardErrorResponseOption.OptionShutdownSystem);

                    WinttCallStack.RegisterReturn();
                }));
                CommandManager.RegisterCommand(new CommandAction(new string[] { "tty" }, User.AccessLevel.Administrator, () =>
                {
                    WinttCallStack.RegisterCall(new("WinttOS.wSystem.WinttOS.Execute()"));

                    if (IsTty)
                    {
                        SystemIO.STDOUT = new ConsoleIO();
                        SystemIO.STDERR = new ConsoleIO();
                        SystemIO.STDIN = new ConsoleIO();

                        if (FullScreenCanvas.IsInUse)
                            FullScreenCanvas.Disable();

                        Tty = default;
                        IsTty = false;
                    }
                    else
                    {
                        SystemIO.STDOUT = new TtyIO();
                        SystemIO.STDERR = new TtyIO();
                        SystemIO.STDIN = new TtyIO();

                        Tty = new(1920, 1080);
                        IsTty = true;
                    }

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

            ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.NONE, "Threads");

            CoroutinePool.Main.PerformHeapCollection = false;

            CoroutinePool.Main.AddCoroutine(new(SystemThread()));

            CoroutinePool.Main.AddCoroutine(new(GarbageCollector()));

            CoroutinePool.Main.AddCoroutine(new(ProcessManager.UpdateProcesses()));
            
            Console.Clear();
            try
            {
                ShellUtils.PrintTaskResult("Starting", ShellTaskResult.NONE, "ThreadPool");

                KernelPrint = false;

                // All after-init code which only runs once goes here.

                SystemIO.STDOUT.PutLine("\n\nWelcome to WinttOS!\n"); // welcome message
                SystemIO.STDOUT.PutLine("To switch to VGA console type 'tty'");
                SystemIO.STDOUT.PutLine("BEWARE: All text on screen will be erased on switch");

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

        private static void InitUsers()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.WinttOS.InitUSers()",
                "void()", "WinttOS.cs", 86));

            if (!UsersManager.TryLoadUsersData())
            {
                UsersManager.AddUser(new User.UserBuilder().SetUserName("root")
                                                           .SetAccess(User.AccessLevel.Administrator)
                                                           .Build());
                UsersManager.TryLoginIntoUserAccount("root", null);
            }
            if(UsersManager.RootUser.HasPassword)
            {
                tryMore:
                Console.Write("Please enter password from user 'root': ");
                if (!UsersManager.TryLoginIntoUserAccount("root", Console.ReadLine()))
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

        public static IEventBus GetGlobalEventBus() => _globalEventBus;
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

            WinttDebugger.Trace("3 seconds elapsed, running shutdown tasks, and finishing running coroutines!", instance);

            SystemTaskScheduler.CallShutdownSchedule();

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
                        if (process.CurrentSet == PrivilegesSet.NONE || process.HasOwnerProcess)
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
