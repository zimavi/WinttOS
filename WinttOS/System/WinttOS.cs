using Cosmos.Core.Memory;
using Cosmos.HAL;
using Cosmos.HAL.Drivers.Audio;
using Cosmos.System.Audio.IO;
using Cosmos.System.Audio;
using Cosmos.System.Coroutines;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.System;
using WinttOS.System.Processing;
using WinttOS.System.Services;
using WinttOS.System.Users;
using WinttOS.System.wosh;
using WinttOS.System.wosh.commands;
using WinttOS.System.wosh.commands.Misc;
using Sys = Cosmos.System;

namespace WinttOS.System
{
    public class WinttOS
    {
        #region Fields

        private static WinttOS instance => new();

        public const string WinttVersion = "WinttOS v1.0.0 dev. build 1987";

        public static WinttServiceProvider ServiceProvider =>
            (WinttServiceProvider)ProcessManager.GetProcessInstance(serviceProviderProcessID);

        public static UsersManager UsersManager { get; set; } = new(null);
        public static ProcessManager ProcessManager { get; set; } = new();
        public static CommandManager CommandManager { get; set; } = new();
        public static bool IsSleeping { get; set; } = false;
        public static List<Action> OnSystemSleep { get; private set; } = new();

        private static WinttServiceProvider serviceProvider = new();
        private static uint serviceProviderProcessID = 0;

        #endregion

        #region Methods

        public static void InitializeSystem()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.WinttOS.InitializeSystem()",
                "void()", "WinttOS.cs", 40));
            Kernel.OnKernelFinish.Add(SystemFinish);
            InitNetwork();
            InitUsers();
            InitServiceProvider();

            ProcessManager.RegisterProcess(serviceProvider, ref serviceProviderProcessID);

            CommandManager.RegisterCommand(new DevModeCommand("dev-mode"));
            CommandManager.RegisterCommand(new ExampleCrashCommand("crash-pls"));

            ((WinttServiceProvider)ProcessManager.GetProcessInstance(serviceProviderProcessID))
                .AddService(CommandManager);

            ProcessManager.StartProcess(serviceProviderProcessID);

            Heap.Collect();

            CoroutinePool.Main.PerformHeapCollection = false;

            CoroutinePool.Main.OnCoroutineCycle.Add(SystemThread);

            CoroutinePool.Main.AddCoroutine(new(ProcessManager.UpdateProcesses()));
            
            Console.Clear();

            CoroutinePool.Main.StartPool();

            WinttCallStack.RegisterReturn();
        }

        private static void InitServiceProvider()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.WinttOS.InitServiceProvider()",
                "void()", "WinttOS.cs", 75));
            serviceProvider.Initialize();

            serviceProvider.AddService(new PowerManagerService());

            WinttCallStack.RegisterReturn();
        }

        private static void InitUsers()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.WinttOS.InitUSers()",
                "void()", "WinttOS.cs", 86));
            if (!UsersManager.LoadUsersData())
            {
                UsersManager.AddUser(new User.UserBuilder().SetUserName("root")
                                                           .SetAccess(User.AccessLevel.Administrator)
                                                           .Build());
                UsersManager.LoginIntoUserAccount("root", null);
            }
            WinttCallStack.RegisterReturn();
        }

        private static void InitNetwork()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.WinttOS.InitNetwork()",
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
                    WinttDebugger.Critical(e.Message, true, instance);
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
            WinttCallStack.RegisterCall(new("WinttOS.System.WinttOS.SystemFinish()",
                "void()", "WinttOS.cs", 131));
            CoroutinePool.Main.AddCoroutine(new(FinishOS()));
            WinttCallStack.RegisterReturn();
        }

        private static IEnumerator<CoroutineControlPoint> FinishOS()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.WinttOS.FinishOS()",
                "IEnumerator<CoroutineControlPoint>()", "WinttOS.cs", 139));

            WinttDebugger.Trace("FinishOS corouting executed! Waiting 3 seconds!", instance);

            WinttCallStack.RegisterReturn();

            yield return WaitFor.Seconds(3);

            WinttCallStack.RegisterCall(new("WinttOS.System.WinttOS.FinishOS()",
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

        public static void SystemThread()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.WinttOS.SystemThread()",
                "void()", "WinttOS.cs", 165));

            Heap.Collect();

            WinttCallStack.RegisterReturn();
        }

        #endregion
    }
}
