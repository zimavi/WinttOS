using Cosmos.Core.Memory;
using Cosmos.HAL;
using Cosmos.HAL.Drivers.Video;
using Cosmos.System.Coroutines;
using Cosmos.System.Graphics;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using System;
using System.Collections.Generic;
using System.Text;
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


        public static UsersManager UsersManager { get; set; } = new(null);
        public static ProcessManager ProcessManager { get; set; } = new();
        public static WinttServiceProvider ServiceProvider { get; set; } = new();

        public static CommandManager CommandManager { get; set; } = new();

        #endregion

        #region Methods

        public static void InitializeSystem()
        {
            Kernel.OnKernelFinish.Add(SystemFinish);

            InitNetwork();
            InitUsers();
            InitServiceProvider();

            uint servicePrvID = 0;

            ProcessManager.RegisterProcess(ServiceProvider, ref servicePrvID);

            CommandManager.registerCommand(new DevModeCommand("dev-mode"));
            CommandManager.registerCommand(new ExampleCrashCommand("crash-pls"));
            CommandManager.registerCommand(new UsersCommand("users"));

            ServiceProvider.AddService(CommandManager);

            //ProcessManager.RegisterProcess(man);

            ProcessManager.StartProcess(0);

            Heap.Collect();

            CoroutinePool.Main.PerformHeapCollection = false;

            CoroutinePool.Main.OnCoroutineCycle.Add(SystemThread);

            CoroutinePool.Main.AddCoroutine(new(ProcessManager.UpdateProcesses()));

            CoroutinePool.Main.StartPool();
            
        }

        private static void InitServiceProvider()
        {
            ServiceProvider.Initialize();

            ServiceProvider.AddService(new TestService());
        }

        private static void InitUsers()
        {
            if(!UsersManager.LoadUsersData())
            {
                UsersManager.AddUser(new User.UserBuilder().SetUserName("root")
                                                           .SetAccess(User.AccessLevel.Administrator)
                                                           .Build());
                UsersManager.LoginIntoUserAccount("root", null);
            }
        }

        private static void InitNetwork()
        {
            Console.WriteLine("NOTE! If you have more then one network apadters, please remove all except one!\n");

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
        }

        public static void SystemFinish()
        {
            CoroutinePool.Main.AddCoroutine(new(FinishOS()));
        }

        private static IEnumerator<CoroutineControlPoint> FinishOS()
        {
            WinttDebugger.Trace("FinishOS corouting executed! Waiting 3 seconds!", instance);
            yield return WaitFor.Seconds(3);
            WinttDebugger.Trace("3 seconds elapsed, finishing running coroutines!", instance);
            foreach (var coroutine in CoroutinePool.Main.RunningCoroutines)
            {
                coroutine.Stop();
            }
            WinttDebugger.Info("Finishing Kernel!", instance);
            if (Kernel.isRebooting)
                Sys.Power.Reboot();
            else
                Sys.Power.Shutdown();
        }

        public static void SystemThread()
        {
            //WinttDebugger.Trace("Collected " + Heap.Collect() + " heap objects");
            Heap.Collect();
        }

        #endregion
    }
}
