using Cosmos.Core.Memory;
using Cosmos.HAL;
using Cosmos.System.Coroutines;
using Cosmos.System.Graphics;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using System;
using System.Collections.Generic;
using System.IO;
using WinttOS.Core;
using WinttOS.Core.Utils.Cryptography;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.GUI;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Processing;
using WinttOS.wSystem.Scheduling;
using WinttOS.wSystem.Services;
using WinttOS.wSystem.Shell;
using WinttOS.wSystem.Users;
using WinttOS.wSystem.wAPI;
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

        public static readonly string WinttVersion = "WinttOS v1.4.0 dev.";
        public static readonly string WinttRevision = VersionInfo.revision;

        public static WinttServiceManager ServiceManager { get; internal set; }
        public static TaskScheduler SystemTaskScheduler { get; private set; }
        public static UsersManager UsersManager { get; /*private */set; }
        public static ProcessManager ProcessManager { get; private set; }
        public static CommandManager CommandManager { get; internal set; }
        public static PackageManager PackageManager { get; private set; }
        public static WindowManager WindowManager { get; internal set; }
        public static Memory MemoryManager { get; private set; }

        public static bool IsTty { get; set; } = false;
        public static Tty Tty { get; set; }
        public static bool IsSleeping { get; set; } = false;
        public static bool KernelPrint { get; internal set; } = true;

        public static string BootTime { get; private set; }

        private static List<Action> OnSystemSleep;


        private static WinttServiceManager serviceManager;

        private static EventBus _globalEventBus;

        private static SystemD systemd;

        public static Canvas SystemCanvas { get; private set; }

        #endregion

        #region Methods

        public static void InitializeSystem()
        {
            try
            {
                BootTime = Time.DayString() + "/" + Time.MonthString() + "/" + Time.YearString() + ", " + Time.TimeString(true, true, true);

                ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.NONE, "Standard IO");

                Logger.DoOSLog("[Info] Initializing standard IO");

                SystemIO.STDOUT = new TtyIO();
                SystemIO.STDERR = new TtyIO();
                SystemIO.STDIN = new TtyIO();

                Logger.DoOSLog("[OK] STDOUT, STDIN, STDERR initalized");
                Logger.DoOSLog("[Info] Starting TTY");

                ShellUtils.PrintTaskResult("Starting", ShellTaskResult.NONE, "TTY");

                //Tty = new(1280, 1024);       // HD
                Tty = new(1920, 1080);    // FullHD
                
                IsTty = true;

                Logger.DoOSLog("[OK] TTY started");


                Logger.DoOSLog("[Info] Initializing task schedular");

                SystemTaskScheduler = new TaskScheduler();

                Logger.DoOSLog("[Info] Initializing user manager");

                UsersManager = new UsersManager();

                Logger.DoOSLog("[Info] Initializing process manager");

                ProcessManager = new ProcessManager();

                Logger.DoOSLog("[Info] Initializing memory manager");

                MemoryManager = new Memory();

                Logger.DoOSLog("[Info] Starting Systemd");

                systemd = new SystemD();

                if(!ProcessManager.TryRegisterProcess(systemd, out uint systemdId))
                {
                    Kernel.WinttRaiseHardError("Unable to start systemd", instance);
                }

                ProcessManager.TryStartProcess(systemdId);

                Logger.DoOSLog("[Info] Initializing package manager");

                PackageManager.Initialize();

                OnSystemSleep = new List<Action>();

                Kernel.OnKernelFinish.Add(SystemFinish);

                Logger.DoOSLog("[Info] Initalizing network");

                InitNetwork();

                Logger.DoOSLog("[Info] Trying to load user data");

                UsersManager.LoadUsers();
            }
            catch(Exception e)
            {
                Kernel.WinttRaiseHardError("Exception accured during system init: " + e.Message, instance);
            }

            Heap.Collect();

            ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.NONE, "Coroutines");

            Logger.DoOSLog("[Info] Initializing coroutine pool");

            CoroutinePool.Main.PerformHeapCollection = false;

            CoroutinePool.Main.AddCoroutine(new(SystemThread()));

            CoroutinePool.Main.AddCoroutine(new(GarbageCollector()));

            CoroutinePool.Main.AddCoroutine(new(ProcessManager.UpdateProcesses()));
            
            Console.Clear();
            try
            {
                ShellUtils.PrintTaskResult("Starting", ShellTaskResult.NONE, "Coroutine pool");

                KernelPrint = false;

                Login();

                // All after-init code which only runs once goes here.
                TestService service = new();
                ServiceManager.AddService(service);
                ServiceManager.StartService(service.ProcessName);

                SystemIO.STDOUT.PutLine("\n\nWelcome to WinttOS!\n"); // welcome message
                SystemIO.STDOUT.PutLine("To switch to VGA console type 'tty'");
                SystemIO.STDOUT.PutLine("BEWARE: All text on screen will be erased on switch");

                for(int i = 0; i < 16; i++)
                {
                    Tty.Foreground = (ConsoleColor)i;
                    SystemIO.STDOUT.Put(new string(new char[] { (char)0x0db, (char)0x0db }));
                }

                Tty.Foreground = ConsoleColor.White;
                SystemIO.STDOUT.PutLine("");

                Logger.DoOSLog("[OK] Initialize finished!");
                Logger.DoOSLog("[Info] Starting pool");
                Logger.DoLogCosmos = false;
                CoroutinePool.Main.StartPool();
            }
            catch(Exception e)
            {
                Kernel.WinttRaiseHardError(e.Message, instance);
            }
        }

        private static void Login()
        {
            SystemIO.STDOUT.Put("Login prompt.\nEnter username: ");
            string username = SystemIO.STDIN.Get();
            SystemIO.STDOUT.Put("\nEnter password:\n");
            string password = SystemIO.STDIN.Get(true);

            string hash = Sha256.hash(password);

            UsersManager.LoadUsers();

            if (UsersManager.GetUser("user:" + username).Contains(hash))
            {
                UsersManager.loggedIn = true;
                UsersManager.userLogged = username;
                if (username != "root")
                {
                    UsersManager.userDir = @"0:\home\" + username + @"\";
                    GlobalData.CurrentDirectory = UsersManager.userDir;
                    UsersManager.LoggedLevel = AccessLevel.FromName(UsersManager.GetUser("user:" + username).Split('|')[1]);
                }
                else
                {
                    UsersManager.userDir = @"0:\root\";

                    UsersManager.LoggedLevel = AccessLevel.SuperUser;

                    if (!Directory.Exists(@"0:\root\"))
                        Directory.CreateDirectory(@"0:\root\");
                }
            }
        }
        private static void InitNetwork()
        {
            //Console.WriteLine("NOTE! If you have more then one network adapters, please remove all except one!\n");

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
                    Logger.DoOSLog("[Error] Network init -> " + e.Message);
                    throw;
                }
            }
            else
                ShellUtils.PrintTaskResult("Discovering IP address", ShellTaskResult.FAILED);
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
            CoroutinePool.Main.AddCoroutine(new(FinishOS()));
        }

        private static IEnumerator<CoroutineControlPoint> FinishOS()
        {

            Logger.DoOSLog("[Info] Stop thread started!");

            ShellUtils.PrintTaskResult("Running", ShellTaskResult.DOING, "System finish");

            Logger.DoOSLog("[Info] Doing 3 seconds cooldown");

            yield return WaitFor.Seconds(3);

            Logger.DoOSLog("[Info] Continue shutdown sequence");

            ShellUtils.MoveCursorUp();

            ShellUtils.PrintTaskResult("Running", ShellTaskResult.OK, "System finish");

            SystemTaskScheduler.CallShutdownSchedule();

            ShellUtils.PrintTaskResult("Stopping", ShellTaskResult.DOING, "Running threads");

            foreach (var coroutine in CoroutinePool.Main.RunningCoroutines)
            {
                coroutine.Stop();
            }

            ShellUtils.MoveCursorUp();
            ShellUtils.PrintTaskResult("Stopping", ShellTaskResult.OK, "Running threads");

            Logger.DoOSLog("[Info] Goodbye!");

            SystemIO.STDOUT.PutLine("Is now safe to turn off your computer!");

            if (Kernel.IsRebooting)
                Sys.Power.Reboot();
            else
                Sys.Power.Shutdown();
        }

        public static IEnumerator<CoroutineControlPoint> SystemThread()
        {
            while (!Kernel.IsFinishingKernel)
            {
                MemoryManager.Monitor();

                yield return WaitFor.Seconds(3);
            }

            yield return WaitFor.Seconds(2);
        }

        public static IEnumerator<CoroutineControlPoint> GarbageCollector()
        {
            ProcessManager.RunProcessGC();
            ServiceManager.RunServiceGC();

            Heap.Collect();

            yield return WaitFor.Milliseconds(10000);

        }

        #endregion
    }
}
