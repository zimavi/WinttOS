using System;
using WinttOS.Core;
using Sys = Cosmos.System;
using WinttOS.Core.Programs.RunCommands;
using WinttOS.Core.commands;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using Cosmos.HAL;
using WinttOS.Core.Utils;
using WinttOS.Core.Utils.Debugging;
using Cosmos.Core.Memory;
using WinttOS.Core.Users;
using System.Collections.Specialized;
using Cosmos.System.Coroutines;
using System.Collections.Generic;
using WinttOS.Core.Services;
using System.Linq;

namespace WinttOS
{
    public class Kernel : Sys.Kernel
    {
        public static readonly string Version = "WinttOS v0.1.0-dev, build 786";
        public static StringCollection ReadonlyFiles { get; internal set; }
        public static StringCollection ReadonlyDirectories { get; internal set; }

        public static UsersManager UsersManager { get; internal set; } = new(null);

        protected override void BeforeRun()
        {
            try
            {
                ReadonlyFiles = new()
                {
                    @"0:\Root.txt",
                    @"0:\Kudzu.txt",
                };

                ReadonlyDirectories = new()
                {
                    @"0:\WinttOS",
                    @"0:\WinttOS\System32"
                };
                

                ShellUtils.MoveCursorUp(-1);

                ShellUtils.PrintTaskResult("Registration File System", ShellTaskResult.DOING);

                GlobalData.fs = new Sys.FileSystem.CosmosVFS();
                Sys.FileSystem.VFS.VFSManager.RegisterVFS(GlobalData.fs);
                GlobalData.fs.Initialize(true);

                ShellUtils.MoveCursorUp();
                ShellUtils.PrintTaskResult("Registration File System", ShellTaskResult.OK);



                ShellUtils.PrintTaskResult("Registrating commands", ShellTaskResult.DOING);

                manager = new CommandManager();

                ShellUtils.MoveCursorUp();
                ShellUtils.PrintTaskResult("Registrating commands", ShellTaskResult.OK);

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
                        WinttDebugger.Critical(e.Message, true, this);
                    }
                }
                else
                    ShellUtils.PrintTaskResult("Discovering IP address", ShellTaskResult.FAILED);

                Console.WriteLine("System initialization complete!");

                Console.WriteLine("WinttOS loaded successfully!\n");

                Global.PIT.Wait(500);

                Console.Clear();


                manager.registerCommand(new mivCommand("miv"));
                manager.registerCommand(new CatUtilCommand("cat"));
                manager.registerCommand(new DevModeCommand("dev-mode"));
                manager.registerCommand(new ExampleCrashCommand("crash-pls"));
                manager.registerCommand(new WgetCommand("wget"));

                if(!UsersManager.LoadUsersData())
                {
                    Console.WriteLine("As OS did not find user's data, please specify new password for \"root\" superuser");
                    Console.Write("Password: ");
                    string pass = Console.ReadLine();
                    UsersManager.AddUser(new User.UserBuilder().SetUserName("root")
                                                               .SetPassword(pass)
                                                               .SetAccess(User.AccessLevel.Administrator)
                                                               .Build());
                    Console.Clear();
                    Console.Write("Do you want to create antoher account? (Y/n): ");
                    bool _loop = true;
                    while(_loop)
                    {
                        string name;
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Y:
                                Console.Clear();
                                Console.Write("Please enter new user name: ");
                                name = Console.ReadLine();
                                Console.Write("Please enter new user's password: ");
                                pass = Console.ReadLine();
                                if (!string.IsNullOrEmpty(pass))
                                    UsersManager.AddUser(new User.UserBuilder().SetUserName(name)
                                                                               .SetPassword(pass)
                                                                               .Build());
                                else
                                    UsersManager.AddUser(new User.UserBuilder().SetUserName(name)
                                                                               .Build());
                                UsersManager.LoginIntoUserAccount(name, pass);
                                break;
                            case ConsoleKey.N:
                                _loop = false;
                                UsersManager.LoginIntoUserAccount("root", pass);
                                break;
                            case ConsoleKey.Enter:
                                Console.Clear();
                                Console.Write("Please enter new user name: ");
                                name = Console.ReadLine();
                                Console.Write("Please enter new user's password: ");
                                pass = Console.ReadLine();
                                if (!string.IsNullOrEmpty(pass))
                                    UsersManager.AddUser(new User.UserBuilder().SetUserName(name)
                                                                               .SetPassword(pass)
                                                                               .Build());
                                else
                                    UsersManager.AddUser(new User.UserBuilder().SetUserName(name)
                                                                               .Build());
                                UsersManager.LoginIntoUserAccount(name, pass);
                                _loop = false;
                                break;
                            default:
                                Console.Clear();
                                break;
                        }
                    }
                    Console.Clear();
                }

                WinttDebugger.Trace("Cleanned " + Heap.Collect() + " objects", this);

                CoroutinePool.Main.AddCoroutine(new(serviceHandler()));
                CoroutinePool.Main.OnCoroutineCycle.Add(ThreadCycleFinish);
                CoroutinePool.Main.StartPool();

            } catch (Exception ex)
            {
                WinttDebugger.Critical(ex.Message, true, this);
            }
        }

        public static CommandManager manager { get; private set; }

        public static bool FinishingKernel { get; private set; } = false;
        private static bool isRebooting = false;

        IEnumerator<CoroutineControlPoint> serviceHandler()
        {
            WinttKernelServiceProvider provider = new();
            provider.AddService(new TestService());

            provider.StartAllServices();
            while(true)
            {
                provider.DoServiceProviderTick();

                if (FinishingKernel)
                    break;

                yield return null;
            }
            provider.StopAllServices();
        }

        private bool didRunCycle = true;

        private void ThreadCycleFinish()
        {
            try
            {
                if (didRunCycle)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(@$"{UsersManager.currentUser.Name}$0:\{GlobalData.currDir}> ");
                    Console.ForegroundColor = ConsoleColor.White;
                    didRunCycle = false;
                }
                string input = "";
                bool hasKey = ShellUtils.ProcessExtendedShellInput(ref input);
                if (hasKey)
                {
                    string[] split = input.Split(' ');
                    Console.ForegroundColor = ConsoleColor.Gray;
                    string response = manager.processInput(input);
                    Console.WriteLine(response);
                    didRunCycle = true;
                }
                WinttDebugger.Trace("Cleanned " + Heap.Collect() + " objects", this);
                if (FinishingKernel)
                    CoroutinePool.Main.AddCoroutine(new(KernelShutdown()));
            }
            #region Catch
            catch (Exception ex)
            {
                WinttDebugger.Error("", false);
                WinttDebugger.Error("", false);
                WinttDebugger.Error("DETECTED SYSTEM CRASH!", false);
                WinttDebugger.Error("", false);
                WinttDebugger.Error("", false);
                WinttDebugger.Critical(ex.Message, ex, this);
            }
            #endregion
        }

        private IEnumerator<CoroutineControlPoint> KernelShutdown()
        {
            while (true)
            {
                if (CoroutinePool.Main.RunningCoroutines.Count() == 1)
                    break;
                yield return WaitFor.Seconds(3);
            }
            if (isRebooting)
                Sys.Power.Reboot();
            else
                Sys.Power.Shutdown();
        }

        protected override void Run()
        {
            
        }

        protected override void AfterRun()
        {
            UsersManager.SaveUsersData();
            Console.WriteLine("Is now safe to turn off your computer!");
            Sys.Power.Shutdown();
            base.AfterRun();
        }


        public static void ShutdownKernel()
        {
            Console.Clear();
            if(!isRebooting)
                Console.WriteLine("Shutting down...");
            else
                Console.WriteLine("Rebooting...");
            UsersManager.SaveUsersData();
            FinishingKernel = true;
        }

        public static void RebootKernel()
        {
            isRebooting = true;
            ShutdownKernel();
        }
    }
}
